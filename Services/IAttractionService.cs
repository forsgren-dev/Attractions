using Models.DTO;

namespace Services;

public interface IAttractionService
{
    public Task SeedAsync(int nrItems);
    public Task<ResponsePageDto<AttractionDto>> ListAsync(int pageSize, int pageNumber);
    public Task<ResponseItemDto<AttractionDto>> ReadAttractionAsync(Guid id);
}
