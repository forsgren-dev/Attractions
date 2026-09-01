using Microsoft.Extensions.Logging;

using DbRepos;

namespace Services;
    
public class CommentServiceDb : ICommentService
{
    private readonly CommentDbRepos _repo = null;
    private readonly ILogger<CommentServiceDb> _logger = null;

    //public Task SeedAsync(int nrItems) => _repo.SeedAsync(nrItems);

    #region constructors
    public CommentServiceDb(CommentDbRepos repo)
    {
        _repo = repo;
    }
    public CommentServiceDb(CommentDbRepos repo, ILogger<CommentServiceDb> logger):this(repo)
    {
        _logger = logger;
    }
    #endregion
}

