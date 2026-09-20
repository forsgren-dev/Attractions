namespace Models.DTO;

public class CommentDto
{
    public Guid CommentId { get; set; }
    public string CommentText { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}
