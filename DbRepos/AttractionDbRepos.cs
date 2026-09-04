using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;
using Models;

namespace DbRepos;

public class AttractionDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AttractionDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task SeedAsync(int nrItems)
    {
        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();

        for (int i = 0; i < nrItems; i++)
        {
            var countryName = seeder.Country;
            var address = SeedAddress(seeder, countryName);

            var attraction = new AttractionDbM
            {
                AttractionId = Guid.NewGuid(),
                AttractionName = seeder.MusicGroupName,
                AttractionDescription = seeder.LatinSentence,
                AddressDbM = address,
                CategoryDbM = SeedCategories(seeder)
            };

            _dbContext.Attractions.Add(attraction);
        }

        await _dbContext.SaveChangesAsync();
    }

    private AddressDbM SeedAddress(SeedGenerator seeder, string countryName)
    {
        var country = new CountryDbM
        {
            CountryId = Guid.NewGuid(),
            CountryName = countryName
        };

        var city = new CityDbM
        {
            CityId = Guid.NewGuid(),
            CityName = seeder.City(countryName),
            CountryDbM = country
        };

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
        var pickedCategories = Enum.GetValues<CategoryType>()
            .OrderBy(_ => seeder.Next())
            .Take(nrOfCategories);

        return pickedCategories.Select(categoryType => new CategoryDbM
        {
            CategoryId = Guid.NewGuid(),
            CategoryType = categoryType
        }).ToList();
    }

    public AttractionDbRepos(
        ILogger<AttractionDbRepos> logger,
        Encryptions encryptions,
        MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}
