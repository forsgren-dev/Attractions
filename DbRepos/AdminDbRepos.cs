using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    // public async Task SeedAsync(int nrItems)
    // {
    //     }

    public async Task RemoveSeededAsync()
    {
        var seededAttractions = await _dbContext.Attractions
            .Include(a => a.AddressDbM)
                .ThenInclude(a => a.CityDbM)
                    .ThenInclude(c => c.CountryDbM)
            .Include(a => a.CategoryDbM)
            .Include(a => a.CommentDbM)
            .Where(a => a.Seeded == true)
            .ToListAsync();

        var seededAddresses = seededAttractions
            .Where(a => a.AddressDbM is not null)
            .Select(a => a.AddressDbM)
            .Distinct()
            .ToList();

        var seededCities = seededAddresses
            .Where(a => a.CityDbM is not null)
            .Select(a => a.CityDbM)
            .Distinct()
            .ToList();

        var seededCountries = seededCities
            .Where(c => c.CountryDbM is not null)
            .Select(c => c.CountryDbM)
            .Distinct()
            .ToList();

        var seededCategories = seededAttractions
            .SelectMany(a => a.CategoryDbM)
            .Distinct()
            .ToList();

        var attractionComments = seededAttractions
            .SelectMany(a => a.CommentDbM)
            .Distinct()
            .ToList();

        var seededUsers = await _dbContext.Users
            .Where(u => u.Seeded == true)
            .Include(u => u.CommentDbM)
            .ToListAsync();

        var userComments = seededUsers
            .SelectMany(u => u.CommentDbM)
            .Distinct()
            .ToList();

        _dbContext.Comments.RemoveRange(attractionComments.Union(userComments));
        _dbContext.Attractions.RemoveRange(seededAttractions);
        _dbContext.Categories.RemoveRange(seededCategories);
        _dbContext.Addresses.RemoveRange(seededAddresses);
        _dbContext.Cities.RemoveRange(seededCities);
        _dbContext.Countries.RemoveRange(seededCountries);
        _dbContext.Users.RemoveRange(seededUsers);

        await _dbContext.SaveChangesAsync();
    }

    public AdminDbRepos(
        ILogger<AdminDbRepos> logger,
        Encryptions encryptions,
        MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}
