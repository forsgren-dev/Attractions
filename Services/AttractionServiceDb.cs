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
        string country = null) =>
        _repo.ReadAttractionsAsync(pageSize, pageNumber, attractionName, category, description, city, country);
        
    public Task<ResponsePageDto<AttractionDto>> ReadAttractionsNoCommentsAsync(
        int pageSize,
        int pageNumber,
        string city = null,
        string country = null) =>
        _repo.ReadAttractionsNoCommentsAsync(pageSize, pageNumber, city, country);

    public Task<ResponseItemDto<AttractionDto>> ReadSingleAttractionAsync(Guid id) => _repo.ReadItemAsync(id);

   
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

