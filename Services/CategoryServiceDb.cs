using Microsoft.Extensions.Logging;

using DbRepos;
using Models.DTO;

namespace Services;

public class CategoryServiceDb : ICategoryService
{
    private readonly CategoryDbRepos _repo = null;
    private readonly ILogger<CategoryServiceDb> _logger = null;

    public Task<ResponsePageDto<CategoryDto>> ReadCategoriesAsync(
        int pageSize = 10,
        int pageNumber = 0,
        string categoryName = null) =>
        _repo.ReadCategoriesAsync(pageSize, pageNumber, categoryName);

    public CategoryServiceDb(
        CategoryDbRepos repo,
        ILogger<CategoryServiceDb> logger)
    {
        _repo = repo;
        _logger = logger;
    }
}
