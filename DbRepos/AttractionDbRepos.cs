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

    public async Task SeedAsync(int nrItems, string[] countries = null)
    {
        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();

        for (int i = 0; i < nrItems; i++)
        {
            var countryName = countries is { Length: > 0 }
                ? seeder.FromList(countries.ToList())
                : seeder.Country;
            var address = CreateAddress(seeder, countryName);

            var attraction = new AttractionDbM
            {
                AttractionId = Guid.NewGuid(),
                AttractionName = seeder.MusicGroupName,
                AttractionDescription = seeder.LatinSentence,
                AddressDbM = address,
                CategoryDbM = CreateCategories(seeder)
            };

            _dbContext.Attractions.Add(attraction);
        }

        await _dbContext.SaveChangesAsync();
    }

    private AddressDbM CreateAddress(SeedGenerator seeder, string countryName)
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

    private List<CategoryDbM> CreateCategories(SeedGenerator seeder)
    {
        var nrOfCategories = seeder.Next(1, 4);
        var pickedCategories = new HashSet<CategoryType>();

        while (pickedCategories.Count < nrOfCategories)
        {
            pickedCategories.Add(seeder.FromEnum<CategoryType>());
        }

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
