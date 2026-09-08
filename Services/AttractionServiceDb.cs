using Microsoft.Extensions.Logging;

using DbRepos;
using Models.DTO;

namespace Services;
    
public class AttractionServiceDb : IAttractionService
{
    private readonly AttractionDbRepos _repo = null;
    private readonly ILogger<AttractionServiceDb> _logger = null;

    public Task SeedAsync(int nrItems) => _repo.SeedAsync(nrItems);
    public Task<ResponsePageDto<AttractionDto>> ListAsync(int pageSize, int pageNumber) => _repo.ListAllAttractionsAsync(pageSize, pageNumber);

    public Task<ResponseItemDto<AttractionDto>> ReadAttractionAsync(Guid id) => _repo.ReadAttractionAsync(id);

   
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

