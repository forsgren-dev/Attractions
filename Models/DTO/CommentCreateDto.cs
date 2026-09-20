namespace Models.DTO;

public class CommentCreateDto
{
    public Guid AttractionId { get; set; }
    public Guid UserId { get; set; }
    public string CommentText { get; set; }
    public DateTime CreatedAt { get; set; }
}
