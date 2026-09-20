using Models.DTO;

namespace Services;

public interface IAttractionService
{
    public Task SeedAsync(int nrItems);
    public Task<ResponsePageDto<AttractionDto>> ReadAttractionsAsync(
        int pageSize = 10,
        int pageNumber = 0,
        string attractionName = null,
        string category = null,
        string description = null,
        string city = null,
        string country = null,
        bool showComments = false);
    public Task<ResponsePageDto<AttractionDto>> ReadAttractionsWithNoCommentsAsync(
        int pageSize = 10,
        int pageNumber = 0,
        string city = null,
        string country = null);
    public Task<ResponseItemDto<AttractionDto>> ReadSingleAttractionAsync(
        Guid id,
        int pageSize = 10,
        int pageNumber = 0,
        bool showComments = false);
    public Task<ResponseItemDto<AttractionDto>> CreateAttractionAsync(AttractionCreateDto item);
    public Task<ResponseItemDto<AttractionDto>> UpdateAttractionAsync(AttractionUpdateDto item);
}
