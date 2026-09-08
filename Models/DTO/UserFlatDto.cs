namespace Models.DTO;

using Newtonsoft.Json;

public class UserFlatDto : IUser
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    [JsonIgnore]
    public List<IComment> Comments { get; set; }

}
