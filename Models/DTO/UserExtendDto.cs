namespace Models.DTO;

using Newtonsoft.Json;

public class UserExtendDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public List<IComment> Comments { get; set; }

}
