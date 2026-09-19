namespace Models.DTO;

public class UserUpdateDto
{
    public Guid? UserId { get; set; }
    public string UserName { get; set; }
    public List<Guid?> CommentsId { get; set; } = null;

    public void EnsureValidity()
    {
        if (UserId is null)
        {
            throw new ArgumentException($"{nameof(UserId)} cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(UserName))
        {
            throw new ArgumentException($"{nameof(UserName)} cannot be empty.");
        }
    }
}
