namespace Models.DTO;

public class CommentCreateDto
{
    public Guid AttractionId { get; set; }
    public Guid UserId { get; set; }
    public string CommentText { get; set; }
    public DateTime CreatedAt { get; set; }


    public void EnsureValidity()
    {
        
        if (AttractionId == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(AttractionId)} cannot be empty.");
        }

        if (UserId == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(UserId)} cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(CommentText))
        {
            throw new ArgumentException($"{nameof(CommentText)} cannot be empty.");
        }
    }
}
