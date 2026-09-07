using Models;
using Models.DTO;

namespace Services;

public interface IAttractionService
{
    public Task SeedAsync(int nrItems);
    public Task RemoveSeededAsync();
    public Task<ResponsePageDto<AttractionDto>> ListAsync(int pageSize, int pageNumber);
}
