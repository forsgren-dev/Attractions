using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Models.DTO;
using Configuration;
using Models;

namespace DbRepos;

public class UserDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<UserDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task<ResponsePageDto<UserDto>> ReadAllAsync(int pageSize = 10, int pageNumber = 0, bool flat = false)
    {
        pageSize = Math.Max(1, pageSize);
        pageNumber = Math.Max(0, pageNumber);

        var totalCount = await _dbContext.Users.CountAsync();

        if (!flat)
        {
            var users = await _dbContext.Users
                .AsNoTracking()
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    Comments = u.CommentDbM
                        .Select(c => new UserCommentsDto
                        {
                            CommentId = c.CommentId,
                            CommentText = c.CommentText,
                            AttractionId = c.AttractionDbM.AttractionId,
                            AttractionName = c.AttractionDbM.AttractionName
                        })
                        .ToList()
                })
                .ToListAsync();

            return new ResponsePageDto<UserDto>
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                DbItemsCount = totalCount,
                Items = users
            };
        }
        else
        {
            var users = await _dbContext.Users
                .AsNoTracking()
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .Select(u => new UserDto
                {
                    UserId = u.UserId,
                    UserName = u.UserName
                })
                .ToListAsync();

            return new ResponsePageDto<UserDto>
            {
#if DEBUG
                ConnectionString = _dbContext.dbConnection,
#endif
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                DbItemsCount = totalCount,
                Items = users
            };
        }
    }

    public async Task<ResponseItemDto<UserDto>> ReadUserAsync(Guid id)
    {
        var item = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.UserId == id)
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                UserName = u.UserName,
                Comments = u.CommentDbM
                    .Select(c => new UserCommentsDto
                    {
                        CommentId = c.CommentId,
                        CommentText = c.CommentText,
                        AttractionId = c.AttractionDbM.AttractionId,
                        AttractionName = c.AttractionDbM.AttractionName
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();


        return new ResponseItemDto<UserDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<UserDto>> CreateUserAsync(UserCreateDto itemDto)
    {
        if (await _dbContext.Users.AnyAsync(u => u.UserName == itemDto.UserName))
        {
            throw new ArgumentException($"UserName {itemDto.UserName} already exists.");
        }

        var item = new UserDbM(itemDto);

        _dbContext.Users.Add(item);
        await _dbContext.SaveChangesAsync();

        return await ReadUserAsync(item.UserId);
    }

    private async Task navProp_UserUpdateDto_to_UserDbM(UserUpdateDto itemDtoSrc, UserDbM itemDst)
    {
        List<CommentDbM> comments = null;
        if (itemDtoSrc.CommentsId != null)
        {
            comments = new List<CommentDbM>();
            foreach (var id in itemDtoSrc.CommentsId)
            {
                if (id is null)
                {
                    throw new ArgumentException($"{nameof(itemDtoSrc.CommentsId)} cannot contain null ids.");
                }

                var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
                if (comment == null)
                {
                    throw new ArgumentException($"Item id {id} not existing.");
                }

                comments.Add(comment);
            }
        }

        itemDst.CommentDbM = comments;
    }


    public async Task SeedAsync(int nrItems)
    {
        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();
        var usedUserNames = new HashSet<string>(
            await _dbContext.Users.Select(u => u.UserName).ToListAsync(),
            StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < nrItems; i++)
        {
            var user = SeedUniqueUser(seeder, usedUserNames);

            _dbContext.Users.Add(user);
        }

        await _dbContext.SaveChangesAsync();
    }

    private static UserDbM SeedUniqueUser(SeedGenerator seeder, HashSet<string> usedUserNames)
    {
        const int maxAttempts = 1000;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var user = new UserDbM().Seed(seeder);

            if (usedUserNames.Add(user.UserName))
            {
                return user;
            }
        }

        throw new InvalidOperationException("Could not create a unique username.");
    }


    public UserDbRepos(
        ILogger<UserDbRepos> logger,
        Encryptions encryptions,
        MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}
