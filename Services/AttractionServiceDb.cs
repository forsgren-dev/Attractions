using Microsoft.Extensions.Logging;

using DbRepos;
using Models;

namespace Services;
    
public class AttractionServiceDb : IAttractionService
{
    private readonly AttractionDbRepos _repo = null;
    private readonly ILogger<AttractionServiceDb> _logger = null;

    public Task SeedAsync(int nrItems) => _repo.SeedAsync(nrItems);
    public Task<List<AttractionListItem>> ListAsync() => _repo.ListAsync();

    #region constructors
    public AttractionServiceDb(AttractionDbRepos repo)
    {
        _repo = repo;
    }
    public AttractionServiceDb(AttractionDbRepos repo, ILogger<AttractionServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion
}

