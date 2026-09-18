using Newtonsoft.Json;

namespace Models.DTO;


public class UserExtendDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public List<UserCommentsDto> Comments { get; set; } = new();

}
