using Microsoft.Extensions.Logging;

using DbRepos;
using Models.DTO;

namespace Services;
    
public class AttractionServiceDb : IAttractionService
{
    private readonly AttractionDbRepos _repo = null;
    private readonly ILogger<AttractionServiceDb> _logger = null;

    public Task SeedAsync(int nrItems) => _repo.SeedAsync(nrItems);
    public Task<ResponsePageDto<AttractionDto>> ReadAttractionsAsync(
        int pageSize,
        int pageNumber,
        string attractionName = null,
        string category = null,
        string description = null,
        string city = null,
        string country = null,
        bool? hasComments = null,
        bool showComments = false) =>
        _repo.ReadAttractionsAsync(pageSize, pageNumber, attractionName, category, description, city, country, hasComments, showComments);
        
    public Task<ResponsePageDto<AttractionDto>> ReadAttractionsWithNoCommentsAsync(
        int pageSize,
        int pageNumber,
        string city = null,
        string country = null) =>
        _repo.ReadAttractionsNoCommentsAsync(pageSize, pageNumber, city, country);

    public Task<ResponseItemDto<AttractionDto>> ReadSingleAttractionAsync(
        Guid id,
        int pageSize = 10,
        int pageNumber = 0,
        bool showComments = false) =>
        _repo.ReadItemAsync(id, pageSize, pageNumber, showComments);

    public Task<ResponseItemDto<AttractionUpdateDto>> ReadAttractionDtoAsync(Guid id) =>
        _repo.ReadUpdateDtoAsync(id);

    public Task<ResponseItemDto<AttractionDto>> CreateAttractionAsync(AttractionCreateDto item) =>
        _repo.CreateAttractionAsync(item);

    public Task<ResponseItemDto<AttractionDto>> UpdateAttractionAsync(AttractionUpdateDto item) =>
        _repo.UpdateAttractionAsync(item);

    public Task<ResponseItemDto<AttractionDto>> DeleteAttractionAsync(Guid id) =>
        _repo.DeleteAttractionAsync(id);

   
    #region constructors
    public AttractionServiceDb(
        AttractionDbRepos repo, 
        ILogger<AttractionServiceDb> logger)
    {
        _repo = repo;
        _logger = logger;
    }   
    
    #endregion
}

