namespace Services;

using Models.DTO;

public interface ICommentService
{
    public Task<ResponseItemDto<CommentDto>> CreateCommentAsync(CommentCreateDto item);
}
