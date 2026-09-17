using Microsoft.Extensions.Logging;

using DbRepos;
using Models.DTO;

namespace Services;
    
public class CommentServiceDb : ICommentService
{
    private readonly CommentDbRepos _repo = null;
    private readonly ILogger<CommentServiceDb> _logger = null;

    public Task<ResponsePageDto<CommentDto>> ReadCommentsByAttractionIdAsync(
        Guid attractionId,
        int pageSize = 10,
        int pageNumber = 0) =>
        _repo.ReadCommentsByAttractionIdAsync(attractionId, pageSize, pageNumber);

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

