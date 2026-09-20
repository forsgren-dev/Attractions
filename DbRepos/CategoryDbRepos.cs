using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using DbContext;
using Configuration;
using Models.DTO;

namespace DbRepos;

public class CategoryDbRepos
{
    private readonly ILogger<CategoryDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    public async Task<ResponsePageDto<CategoryDto>> ReadCategoriesAsync(
        int pageSize = 10,
        int pageNumber = 0,
        string categoryName = null)
    {
        pageSize = Math.Max(1, pageSize);
        pageNumber = Math.Max(0, pageNumber);

        categoryName = categoryName?.Trim().ToLower();

        var query = _dbContext.Categories.AsNoTracking();

        if (!string.IsNullOrEmpty(categoryName))
        {
            query = query.Where(c => c.CategoryName.ToLower().Contains(categoryName));
        }

        var totalCount = await query.CountAsync();

        var categories = await query
            .OrderBy(c => c.CategoryName)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            })
            .ToListAsync();

        return new ResponsePageDto<CategoryDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            DbItemsCount = totalCount,
            Items = categories
        };
    }

    public CategoryDbRepos(
        ILogger<CategoryDbRepos> logger,
        Encryptions encryptions,
        MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}
