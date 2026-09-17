using Models.DTO;

namespace Services;

public interface IAttractionService
{
    public Task SeedAsync(int nrItems);
    public Task<ResponsePageDto<AttractionDto>> ListAttractionsAsync(
        int pageSize = 10,
        int pageNumber = 0,
        string attractionName = null,
        string category = null,
        string description = null,
        string city = null,
        string country = null);
    public Task<ResponseItemDto<AttractionDto>> ReadAttractionAsync(Guid id);
}
