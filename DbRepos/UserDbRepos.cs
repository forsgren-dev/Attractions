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

        public async Task SeedAsync(int nrItems)
    {
        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();

        for (int i = 0; i < nrItems; i++)
        {

            var user = new UserDbM
            {
                UserId = Guid.NewGuid(),
                UserName = $"{seeder.FirstName}+{seeder.Next(10, 9000)}",
                Seeded = true
            };

            _dbContext.Users.Add(user);
        }

        await _dbContext.SaveChangesAsync();
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
