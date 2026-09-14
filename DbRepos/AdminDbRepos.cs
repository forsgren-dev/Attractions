using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;
using Models;

namespace DbRepos;

public class AdminDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task SeedAsync(int nrItems)
    {
        const int batchSize = 500;
        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();
        var usedUserNames = new HashSet<string>(
            await _dbContext.Users.Select(u => u.UserName).ToListAsync(),
            StringComparer.OrdinalIgnoreCase);
        var countries = new HashSet<CountryDbM>(
            await _dbContext.Countries.ToListAsync());
        var cities = new HashSet<CityDbM>(
            await _dbContext.Cities
                .Include(c => c.CountryDbM)
                .ToListAsync());

        for (int i = 0; i < nrItems; i++)
        {
            var countryName = seeder.Country;
            var address = SeedAddress(seeder, countryName, countries, cities);

            var attraction = new AttractionDbM
            {
                AttractionId = Guid.NewGuid(),
                AttractionName = seeder.AttractionName,
                AttractionDescription = seeder.LatinSentence,
                Seeded = true,
                AddressDbM = address,
                CategoryDbM = SeedCategories(seeder)
            };

            var user = new UserDbM
            {
                UserId = Guid.NewGuid(),
                UserName = CreateUniqueUserName(seeder, usedUserNames),
                Seeded = true
            };

            _dbContext.Attractions.Add(attraction);
            _dbContext.Users.Add(user);
            if ((i + 1) % batchSize == 0)
            {
                await _dbContext.SaveChangesAsync();
            }
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveSeededAsync()
    {
        var seededAttractions = await _dbContext.Attractions.Where(a => a.Seeded == true).ToListAsync();

        _dbContext.Attractions.RemoveRange(seededAttractions);
        await _dbContext.SaveChangesAsync();
    }

    private AddressDbM SeedAddress(
        SeedGenerator seeder,
        string countryName,
        HashSet<CountryDbM> countries,
        HashSet<CityDbM> cities)
    {
        var countryCheck = new CountryDbM { CountryName = countryName };

        if (!countries.TryGetValue(countryCheck, out var country))
        {
            country = new CountryDbM
            {
                CountryId = Guid.NewGuid(),
                CountryName = countryName
            };

            countries.Add(country);
        }

        var cityName = seeder.City(countryName);
        var cityCheck = new CityDbM
        {
            CityName = cityName,
            CountryDbM = country
        };

        if (!cities.TryGetValue(cityCheck, out var city))
        {
            city = new CityDbM
            {
                CityId = Guid.NewGuid(),
                CityName = cityName,
                CountryDbM = country
            };

            cities.Add(city);
        }

        return new AddressDbM
        {
            AddressId = Guid.NewGuid(),
            Street = seeder.StreetAddress(countryName),
            PostalCode = seeder.ZipCode.ToString(),
            CityDbM = city
        };
    }

    private List<CategoryDbM> SeedCategories(SeedGenerator seeder)
    {
        var nrOfCategories = seeder.Next(1, 4);
        var attractionCategories = Enum.GetValues<CategoryType>()
            .OrderBy(_ => seeder.Next())
            .Take(nrOfCategories);

        return attractionCategories.Select(categoryType => new CategoryDbM
        {
            CategoryId = Guid.NewGuid(),
            CategoryType = categoryType
        }).ToList();
    }

    private static string CreateUniqueUserName(SeedGenerator seeder, HashSet<string> usedUserNames)
    {
        const int maxAttempts = 1000;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            var userName = $"{seeder.FirstName}{seeder.Next(10, 9000)}";

            if (usedUserNames.Add(userName))
            {
                return userName;
            }
        }

        throw new InvalidOperationException("Could not create username.");
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
