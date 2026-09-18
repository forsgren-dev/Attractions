using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;
using Models.DTO;

namespace DbRepos;

public class CommentDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<CommentDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task<ResponsePageDto<CommentDto>> ReadCommentsByAttractionIdAsync(
        Guid attractionId,
        int pageSize = 10,
        int pageNumber = 0)
    {
        pageSize = Math.Max(1, pageSize);
        pageNumber = Math.Max(0, pageNumber);

        var query = _dbContext.Comments
            .AsNoTracking()
            .Where(c => c.AttractionDbM.AttractionId == attractionId);

        var totalCount = await query.CountAsync();

        var comments = await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                CommentText = c.CommentText,
                UserId = c.UserDbM.UserId,
                UserName = c.UserDbM.UserName
            })
            .ToListAsync();

        return new ResponsePageDto<CommentDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            DbItemsCount = totalCount,
            Items = comments
        };
    }

    public async Task SeedAsync(int nrItems)
    {
        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();

        for (int i = 0; i < nrItems; i++)
        {
            var comment = new CommentDbM().Seed(seeder);
            _dbContext.Comments.Add(comment);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task LinkSeededCommentsAsync()
    {
        var comments = await _dbContext.Comments
            .Where(c => c.Seeded)
            .ToListAsync();

        var users = await _dbContext.Users
            .Where(u => u.Seeded)
            .ToListAsync();

        var attractions = await _dbContext.Attractions
            .Where(a => a.Seeded)
            .ToListAsync();

        if (users.Count == 0 || attractions.Count == 0)
        {
            return;
        }

        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();

        foreach (var comment in comments)
        {
            comment.UserDbM = users[seeder.Next(users.Count)];
            comment.AttractionDbM = attractions[seeder.Next(attractions.Count)];
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task SeedAttractionCommentsAsync()
    {
        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();

        var attractions = await _dbContext.Attractions
            .Include(a => a.CommentDbM)
            .Where(a => a.Seeded)
            .ToListAsync();

        var users = await _dbContext.Users
            .Where(u => u.Seeded)
            .ToListAsync();

        if (users.Count == 0)
        {
            return;
        }

        foreach (var attraction in attractions)
        {
            var maxCommentsAdd = Math.Max(0, 20 - attraction.CommentDbM.Count);
            var commentsAdd = seeder.Next(0, maxCommentsAdd + 1);

            for (int i = 0; i < commentsAdd; i++)
            {
                var comment = new CommentDbM().Seed(seeder);
                comment.AttractionDbM = attraction;
                comment.UserDbM = users[seeder.Next(0, users.Count)];

                _dbContext.Comments.Add(comment);
            }
        }

        await _dbContext.SaveChangesAsync();
    }


    public CommentDbRepos(
        ILogger<CommentDbRepos> logger,
        Encryptions encryptions,
        MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}
