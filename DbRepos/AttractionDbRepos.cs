using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;
using Models;
using Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DbRepos;

public class AttractionDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AttractionDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

   // var query = _dbContext.Attractions
    // .AsNoTracking()
    // .Include(a => a.AddressDbM)
    // .ThenInclude(ad => ad.CityDbM)
    // .ThenInclude(c => c.CountryDbM);

    // För ren listning så använder jag mina DTO:s för att undvika att skicka med onödig data och för att kunna forma datan. 
    // Jag använder LINQ för att projicera data från databasen till mina DTO-objekt.
    public async Task<ResponsePageDto<AttractionDto>> ListAllAttractionsAsync(int pageSize, int pageNumber)
    {
        pageSize = Math.Max(1, pageSize);
        pageNumber = Math.Max(0, pageNumber);

        var totalCount = await _dbContext.Attractions.CountAsync();

        var attractions = await _dbContext.Attractions
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .Select(a => new AttractionDto
            {
                AttractionId = a.AttractionId,
                AttractionName = a.AttractionName,
                AttractionDescription = a.AttractionDescription,
                Address = new AttractionAddressDto
                {
                    Street = a.AddressDbM.Street,
                    PostalCode = a.AddressDbM.PostalCode,
                    City = a.AddressDbM.CityDbM.CityName,
                    Country = a.AddressDbM.CityDbM.CountryDbM.CountryName
                }
            })
            .ToListAsync();

        return new ResponsePageDto<AttractionDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            DbItemsCount = totalCount,
            Items = attractions
        };
    }

    public async Task<ResponseItemDto<AttractionDto>> ReadAttractionAsync(Guid id)
    {
        var item = await _dbContext.Attractions
            .AsNoTracking()
            .Select(a => new AttractionDto
            {
                AttractionId = a.AttractionId,
                AttractionName = a.AttractionName,
                AttractionDescription = a.AttractionDescription,
                Address = new AttractionAddressDto
                {
                    Street = a.AddressDbM.Street,
                    PostalCode = a.AddressDbM.PostalCode,
                    City = a.AddressDbM.CityDbM.CityName,
                    Country = a.AddressDbM.CityDbM.CountryDbM.CountryName
                }
            })
            .FirstOrDefaultAsync(a => a.AttractionId == id);

        return new ResponseItemDto<AttractionDto>
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

        for (int i = 0; i < nrItems; i++)
        {
            var countryName = seeder.Country;
            var address = SeedAddress(seeder, countryName);

            var attraction = new AttractionDbM
            {
                AttractionId = Guid.NewGuid(),
                AttractionName = seeder.AttractionName,
                AttractionDescription = seeder.LatinSentence,
                Seeded = true,
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
            CountryId = country.CountryId,
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
        var attractionCategories = Enum.GetValues<CategoryType>()
            .OrderBy(_ => seeder.Next())
            .Take(nrOfCategories);

        return attractionCategories.Select(categoryType => new CategoryDbM
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
