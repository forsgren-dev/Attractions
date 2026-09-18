namespace Models.DTO;

public class UserCommentsDto
{
    public Guid CommentId { get; set; }
    public string CommentText { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}
