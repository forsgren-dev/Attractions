namespace Models.DTO;

public class UserCommentsDto
{
    public Guid CommentId { get; set; }
    public string CommentText { get; set; }
    public Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
}
