namespace Services;

using Models.DTO;

public interface ICommentService
{
    public Task<ResponsePageDto<CommentDto>> ReadCommentsByAttractionIdAsync(
        Guid attractionId, int pageSize = 10, int pageNumber = 0);
}
