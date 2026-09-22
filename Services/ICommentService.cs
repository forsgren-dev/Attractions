namespace Services;

using Models.DTO;

public interface ICommentService
{
    public Task<ResponsePageDto<CommentDto>> ReadCommentsAsync(
        int pageSize = 10,
        int pageNumber = 0,
        Guid? id = null);
    public Task<ResponseItemDto<CommentDto>> CreateCommentAsync(CommentCreateDto item);
    public Task<ResponseItemDto<CommentDto>> DeleteCommentAsync(Guid id);
}
