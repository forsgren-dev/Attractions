using Models.DTO;

namespace Services;

public interface ICategoryService
{
    public Task<ResponsePageDto<CategoryDto>> ReadCategoriesAsync(
        int pageSize = 10,
        int pageNumber = 0,
        string categoryName = null);
}
