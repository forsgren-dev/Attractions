using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Npgsql;

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
        bool? hasComments = null,
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

        if (hasComments == true)
        {
            query = query.Where(a => a.CommentDbM.Any());
        }
        else if (hasComments == false)
        {
            query = query.Where(a => !a.CommentDbM.Any());
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
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (item != null && showComments)
        {
            var commentsQuery = _dbContext.Comments
                .AsNoTracking()
                .Where(c => c.AttractionDbM.AttractionId == id);

            var totalCount = await commentsQuery.CountAsync();

            var comments = await commentsQuery
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
                .ToListAsync();

            item.Comments = comments;
            item.CommentsPage = new ResponsePageDto<CommentDto>
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

        return new ResponseItemDto<AttractionDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            Item = item
        };
    }

    public async Task<ResponseItemDto<AttractionUpdateDto>> ReadUpdateDtoAsync(Guid id)
    {
        var item = await _dbContext.Attractions
            .AsNoTracking()
            .Where(a => a.AttractionId == id)
            .Select(a => new AttractionUpdateDto
            {
                AttractionId = a.AttractionId,
                AttractionName = a.AttractionName,
                AttractionDescription = a.AttractionDescription,
                Street = a.AddressDbM.Street,
                PostalCode = a.AddressDbM.PostalCode,
                City = a.AddressDbM.CityDbM.CityName,
                Country = a.AddressDbM.CityDbM.CountryDbM.CountryName,
                CategoriesId = a.CategoryDbM
                    .Select(c => (Guid?)c.CategoryId)
                    .ToList()
            })
            .FirstOrDefaultAsync();

        return new ResponseItemDto<AttractionUpdateDto>
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
        var city = await GetOrCreateCityAsync(itemDtoSrc.City, itemDtoSrc.Country);

        itemDst.AddressDbM = new AddressDbM
        {
            AddressId = Guid.NewGuid(),
            Street = EnsureCapitalLetter(itemDtoSrc.Street),
            PostalCode = itemDtoSrc.PostalCode?.Trim(),
            CityDbM = city,
            Seeded = false
        };

        itemDst.CategoryDbM = await GetCategoriesAsync(itemDtoSrc.CategoriesId);
    }

    public async Task<ResponseItemDto<AttractionDto>> UpdateAttractionAsync(AttractionUpdateDto itemDto)
    {
        if (itemDto == null)
        {
            throw new ArgumentException($"{nameof(itemDto)} cannot be null.");
        }

        itemDto.EnsureValidity();

        var item = await _dbContext.Attractions
            .Include(a => a.AddressDbM)
            .ThenInclude(a => a.CityDbM)
            .ThenInclude(c => c.CountryDbM)
            .Include(a => a.CategoryDbM)
            .FirstOrDefaultAsync(a => a.AttractionId == itemDto.AttractionId);

        if (item == null)
        {
            throw new ArgumentException($"Item {itemDto.AttractionId} is not existing.");
        }

        item.AttractionName = EnsureCapitalLetter(itemDto.AttractionName);
        item.AttractionDescription = EnsureCapitalLetter(itemDto.AttractionDescription);

        await navProp_AttractionUpdateDto_to_AttractionDbM(itemDto, item);

        _dbContext.Attractions.Update(item);

        await _dbContext.SaveChangesAsync();

        return await ReadItemAsync(item.AttractionId);
    }

    public async Task<ResponseItemDto<AttractionDto>> DeleteAttractionAsync(Guid id)
    {
        var existing = await ReadItemAsync(id, showComments: true);
        if (existing.Item == null)
        {
            throw new ArgumentException($"Item {id} is not existing.");
        }

        await DeleteAttractionByStoredProcedureAsync(id);

        return existing;
    }

    private async Task DeleteAttractionByStoredProcedureAsync(Guid id)
    {
        var connection = _dbContext.Database.GetDbConnection();
        using var command = connection.CreateCommand();

        List<DbParameter> parameters;
        if (connection is MySqlConnection)
        {
            command.CommandText = "supusr_spDeleteAttraction";
            command.CommandType = CommandType.StoredProcedure;
            parameters =
            [
                new MySqlParameter("attractionIdParam", id)
            ];
        }
        else if (connection is NpgsqlConnection)
        {
            command.CommandText = "SELECT supusr.\"spDeleteAttraction\"(@attractionIdParam)";
            command.CommandType = CommandType.Text;
            parameters =
            [
                new NpgsqlParameter("attractionIdParam", id)
            ];
        }
        else
        {
            command.CommandText = "supusr.spDeleteAttraction";
            command.CommandType = CommandType.StoredProcedure;
            parameters =
            [
                new SqlParameter("attractionIdParam", id)
            ];
        }

        command.Parameters.AddRange(parameters.ToArray());

        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        await command.ExecuteNonQueryAsync();
    }

    private async Task navProp_AttractionUpdateDto_to_AttractionDbM(AttractionUpdateDto itemDtoSrc, AttractionDbM itemDst)
    {
        var city = await GetOrCreateCityAsync(itemDtoSrc.City, itemDtoSrc.Country);

        if (itemDst.AddressDbM == null)
        {
            itemDst.AddressDbM = new AddressDbM
            {
                AddressId = Guid.NewGuid(),
                Seeded = false
            };
        }

        itemDst.AddressDbM.Street = EnsureCapitalLetter(itemDtoSrc.Street);
        itemDst.AddressDbM.PostalCode = itemDtoSrc.PostalCode?.Trim();
        itemDst.AddressDbM.CityDbM = city;
        itemDst.CategoryDbM = await GetCategoriesAsync(itemDtoSrc.CategoriesId);
    }

    private async Task<CityDbM> GetOrCreateCityAsync(string city, string country)
    {
        var countryName = EnsureCapitalLetter(country);
        var cityName = EnsureCapitalLetter(city);

        var countryItem = await _dbContext.Countries
            .FirstOrDefaultAsync(c => !c.Seeded && c.CountryName.ToLower() == countryName.ToLower());

        if (countryItem == null)
        {
            countryItem = new CountryDbM
            {
                CountryId = Guid.NewGuid(),
                CountryName = countryName,
                Seeded = false
            };

            _dbContext.Countries.Add(countryItem);
        }

        var cityItem = await _dbContext.Cities
            .Include(c => c.CountryDbM)
            .FirstOrDefaultAsync(c =>
                c.CityName.ToLower() == cityName.ToLower()
                && !c.Seeded
                && c.CountryDbM.CountryName.ToLower() == countryName.ToLower());

        if (cityItem == null)
        {
            cityItem = new CityDbM
            {
                CityId = Guid.NewGuid(),
                CityName = cityName,
                CountryDbM = countryItem,
                Seeded = false
            };

            _dbContext.Cities.Add(cityItem);
        }

        return cityItem;
    }

    private async Task<List<CategoryDbM>> GetCategoriesAsync(List<Guid?> categoryIds)
    {
        if (categoryIds == null)
        {
            return new List<CategoryDbM>();
        }

        var categories = new List<CategoryDbM>();
        foreach (var id in categoryIds.Distinct())
        {
            if (id is null || id == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(categoryIds)} cannot contain empty ids.");
            }

            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id.Value);
            if (category == null)
            {
                throw new ArgumentException($"Category id {id} not existing.");
            }

            categories.Add(category);
        }

        return categories;
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
            await _dbContext.Countries.Where(c => c.Seeded).ToListAsync());

        var cities = new HashSet<CityDbM>(
            await _dbContext.Cities
                .Where(c => c.Seeded)
                .Include(c => c.CountryDbM)
                .ToListAsync());

        foreach (var (countryName, cityName) in seeder.CityCountries)
        {
            var countryCheck = new CountryDbM { CountryName = countryName, Seeded = true };
            if (!countries.TryGetValue(countryCheck, out var country))
            {
                country = new CountryDbM
                {
                    CountryId = Guid.NewGuid(),
                    CountryName = countryName,
                    Seeded = true
                };
                countries.Add(country);
                _dbContext.Countries.Add(country);
            }

            var cityCheck = new CityDbM
            {
                CityName = cityName,
                CountryDbM = country,
                Seeded = true
            };
            if (!cities.Contains(cityCheck))
            {
                var city = new CityDbM
                {
                    CityId = Guid.NewGuid(),
                    CityName = cityName,
                    CountryDbM = country,
                    Seeded = true
                };
                cities.Add(city);
                _dbContext.Cities.Add(city);
            }
        }

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
        var countryCheck = new CountryDbM { CountryName = countryName, Seeded = true };
        if (!countries.TryGetValue(countryCheck, out var country))
            throw new InvalidOperationException($"Seed country {countryName} was not prepared.");

        var cityName = seeder.City(countryName);
        var cityCheck = new CityDbM
        {
            CityName = cityName,
            CountryDbM = country,
            Seeded = true
        };
        if (!cities.TryGetValue(cityCheck, out var city))
            throw new InvalidOperationException($"Seed city {cityName}, {countryName} was not prepared.");

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
