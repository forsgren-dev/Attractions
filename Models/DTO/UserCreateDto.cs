namespace Models.DTO;

public class UserCreateDto
{
    public string UserName { get; set; }

    public void EnsureValidity()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            throw new ArgumentException($"{nameof(UserName)} cannot be empty.");
        }
    }
}
