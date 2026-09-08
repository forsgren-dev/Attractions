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

    public async Task<ResponsePageDto<IUser>> ListAsync(int pageSize, int pageNumber)
    {
        pageSize = Math.Max(1, pageSize);
        pageNumber = Math.Max(0, pageNumber);

        var totalCount = await _dbContext.Users.CountAsync();

        var users = await _dbContext.Users.AsNoTracking()
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync<IUser>();

        return new ResponsePageDto<IUser>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            DbItemsCount = totalCount,
            Items = users
        };
    }

    public async Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id)
    {
        var item = await _dbContext.Users
        .AsNoTracking()
        .Where(a => a.UserId == id)
        .FirstOrDefaultAsync<IUser>();

    
        return new ResponseItemDto<IUser>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
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
            var userName = CreateUniqueUserName(seeder, usedUserNames);

            var user = new UserDbM
            {
                UserId = Guid.NewGuid(),
                UserName = userName,
                Seeded = true
            };

            _dbContext.Users.Add(user);
        }

        await _dbContext.SaveChangesAsync();
    }

    private static string CreateUniqueUserName(SeedGenerator seeder, HashSet<string> usedUserNames)
    {
        string userName;
        int userSuffix = 1;
        
        userName = $"{seeder.FirstName} {seeder.LastName}";
        if (!usedUserNames.Add(userName))
        {
            do
            {
                userName = $"{seeder.FirstName}_{seeder.LastName}{userSuffix}";
                userSuffix++;
            } while (!usedUserNames.Add(userName));
        }
        ;

        return userName;
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
