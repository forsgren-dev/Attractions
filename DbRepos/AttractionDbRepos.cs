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


    // För listning så använder jag mina DTO:s för att undvika att skicka med onödig data och för att kunna forma datan. 
    // Jag använder LINQ för att projicera data från databasen till mina DTO-objekt.
    public async Task<ResponsePageDto<AttractionDto>> ReadAttractionsAsync(
        int pageSize,
        int pageNumber,
        string attractionName = null,
        string category = null,
        string description = null,
        string city = null,
        string country = null,
        bool showComments = false)
    {
        pageSize = Math.Max(1, pageSize);
        pageNumber = Math.Max(0, pageNumber);

        attractionName = attractionName?.Trim().ToLower();
        category = category?.Trim().ToLower();
        description = description?.Trim().ToLower();
        city = city?.Trim().ToLower();
        country = country?.Trim().ToLower();

        var query = _dbContext.Attractions.AsNoTracking();

        if (!string.IsNullOrEmpty(attractionName))
        {
            query = query.Where(a => a.AttractionName.ToLower().Contains(attractionName));
        }

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(a => a.CategoryDbM.Any(c => c.CategoryName.ToLower().Contains(category)));
        }

        if (!string.IsNullOrEmpty(description))
        {
            query = query.Where(a => a.AttractionDescription.ToLower().Contains(description));
        }

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(a => a.AddressDbM.CityDbM.CityName.ToLower().Contains(city));
        }

        if (!string.IsNullOrEmpty(country))
        {
            query = query.Where(a => a.AddressDbM.CityDbM.CountryDbM.CountryName.ToLower().Contains(country));
        }

        var totalCount = await query.CountAsync();

        var attractions = await query
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
                },
                Categories = a.CategoryDbM
                    .Select(c => c.CategoryType.ToString())
                    .ToList(),
                Comments = !showComments ? null : a.CommentDbM.Select(c => new CommentDto
                {
                    CommentId = c.CommentId,
                    CommentText = c.CommentText,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserDbM.UserId,
                    UserName = c.UserDbM.UserName
                })
                        .ToList()
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

    public async Task<ResponsePageDto<AttractionDto>> ReadAttractionsNoCommentsAsync(
        int pageSize,
        int pageNumber,
        string city = null,
        string country = null)
    {
        pageSize = Math.Max(1, pageSize);
        pageNumber = Math.Max(0, pageNumber);

        city = city?.Trim().ToLower();
        country = country?.Trim().ToLower();

        var query = _dbContext.Attractions.AsNoTracking();

        if (!string.IsNullOrEmpty(city))
        {
            query = query.Where(a => a.AddressDbM.CityDbM.CityName.ToLower().Contains(city));
        }

        if (!string.IsNullOrEmpty(country))
        {
            query = query.Where(a => a.AddressDbM.CityDbM.CountryDbM.CountryName.ToLower().Contains(country));
        }

        query = query.Where(a => !a.CommentDbM.Any());

        var totalCount = await query.CountAsync();

        var attractions = await query
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
                },
                Categories = a.CategoryDbM
                    .Select(c => c.CategoryType.ToString())
                    .ToList()
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

    public async Task<ResponseItemDto<AttractionDto>> ReadItemAsync(
        Guid id,
        int pageSize = 10,
        int pageNumber = 0,
        bool showComments = false)
    {
        pageSize = Math.Max(1, pageSize);
        pageNumber = Math.Max(0, pageNumber);

        var item = await _dbContext.Attractions
            .AsNoTracking()
            .Where(a => a.AttractionId == id)
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
                },
                Categories = a.CategoryDbM
                    .Select(c => c.CategoryName)
                    .ToList(),
                Comments = !showComments ? null :
                a.CommentDbM
                        .OrderBy(c => c.CommentId)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                        .Select(c => new CommentDto
                        {
                            CommentId = c.CommentId,
                            CommentText = c.CommentText,
                            CreatedAt = c.CreatedAt,
                            UserId = c.UserDbM.UserId,
                            UserName = c.UserDbM.UserName
                        })
                        .ToList()
            })
            .FirstOrDefaultAsync();

        return new ResponseItemDto<AttractionDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<AttractionDto>> CreateAttractionAsync(AttractionCreateDto itemDto)
    {
        if (itemDto == null)
        {
            throw new ArgumentException($"{nameof(itemDto)} cannot be null.");
        }

        itemDto.EnsureValidity();

        var item = new AttractionDbM(itemDto);
        item.AttractionName = EnsureCapitalLetter(item.AttractionName);
        item.AttractionDescription = EnsureCapitalLetter(item.AttractionDescription);

        await navProp_AttractionCreateDto_to_AttractionDbM(itemDto, item);

        _dbContext.Attractions.Add(item);

        await _dbContext.SaveChangesAsync();

        return await ReadItemAsync(item.AttractionId);
    }

    private async Task navProp_AttractionCreateDto_to_AttractionDbM(AttractionCreateDto itemDtoSrc, AttractionDbM itemDst)
    {
        var countryName = EnsureCapitalLetter(itemDtoSrc.Country);
        var cityName = EnsureCapitalLetter(itemDtoSrc.City);

        var country = await _dbContext.Countries
            .FirstOrDefaultAsync(c => c.CountryName.ToLower() == countryName.ToLower());

        if (country == null)
        {
            country = new CountryDbM
            {
                CountryId = Guid.NewGuid(),
                CountryName = countryName
            };

            _dbContext.Countries.Add(country);
        }

        var city = await _dbContext.Cities
            .Include(c => c.CountryDbM)
            .FirstOrDefaultAsync(c =>
                c.CityName.ToLower() == cityName.ToLower()
                && c.CountryDbM.CountryName.ToLower() == countryName.ToLower());

        if (city == null)
        {
            city = new CityDbM
            {
                CityId = Guid.NewGuid(),
                CityName = cityName,
                CountryDbM = country
            };

            _dbContext.Cities.Add(city);
        }

        itemDst.AddressDbM = new AddressDbM
        {
            AddressId = Guid.NewGuid(),
            Street = EnsureCapitalLetter(itemDtoSrc.Street),
            PostalCode = itemDtoSrc.PostalCode?.Trim(),
            CityDbM = city,
            Seeded = false
        };

        if (itemDtoSrc.CategoriesId == null)
        {
            return;
        }

        var categories = new List<CategoryDbM>();
        foreach (var id in itemDtoSrc.CategoriesId.Distinct())
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(itemDtoSrc.CategoriesId)} cannot contain empty ids.");
            }

            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                throw new ArgumentException($"Category id {id} not existing.");
            }

            categories.Add(category);
        }

        itemDst.CategoryDbM = categories;
    }

    private static string EnsureCapitalLetter(string value)
    {
        value = value?.Trim();

        if (string.IsNullOrEmpty(value))
        {
            return value;
        }

        return char.ToUpperInvariant(value[0]) + value[1..];
    }

    public async Task SeedAsync(int nrItems)
    {
        const int batchSize = 500;
        var fn = Path.GetFullPath(_seedSource);
        var seeder = File.Exists(fn) ? new SeedGenerator(fn) : new SeedGenerator();

        var countries = new HashSet<CountryDbM>(
            await _dbContext.Countries.ToListAsync());

        var cities = new HashSet<CityDbM>(
            await _dbContext.Cities
                .Include(c => c.CountryDbM)
                .ToListAsync());

        var categories = new HashSet<CategoryDbM>(
            await _dbContext.Categories.ToListAsync());

        for (int i = 0; i < nrItems; i++)
        {
            var countryName = seeder.Country;
            var address = SeedAddress(seeder, countryName, countries, cities);

            var attraction = new AttractionDbM().Seed(seeder);
            attraction.AddressDbM = address;
            attraction.CategoryDbM = SeedCategories(seeder, categories);

            _dbContext.Attractions.Add(attraction);

            if ((i + 1) % batchSize == 0)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

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
            CityDbM = city,
            Seeded = true
        };
    }

    private List<CategoryDbM> SeedCategories(
        SeedGenerator seeder,
        HashSet<CategoryDbM> categories)
    {
        var nrOfCategories = seeder.Next(1, 4);
        var attractionCategories = Enum.GetValues<CategoryType>()
            .OrderBy(_ => seeder.Next())
            .Take(nrOfCategories);

        return attractionCategories.Select(categoryType =>
        {
            var categoryCheck = new CategoryDbM { CategoryType = categoryType };

            if (!categories.TryGetValue(categoryCheck, out var category))
            {
                category = new CategoryDbM
                {
                    CategoryId = Guid.NewGuid(),
                    CategoryType = categoryType
                };

                categories.Add(category);
            }

            return category;
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
