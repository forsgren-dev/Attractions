using Microsoft.Extensions.Logging;

using DbRepos;
using Models.DTO;

namespace Services;
    
public class CommentServiceDb : ICommentService
{
    private readonly CommentDbRepos _repo = null;
    private readonly ILogger<CommentServiceDb> _logger = null;

    public Task<ResponsePageDto<CommentDto>> ReadCommentsAsync(
        int pageSize = 10,
        int pageNumber = 0,
        Guid? id = null) =>
        _repo.ReadCommentsAsync(pageSize, pageNumber, id);

    public Task<ResponseItemDto<CommentDto>> CreateCommentAsync(CommentCreateDto item) =>
        _repo.CreateCommentAsync(item);

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

