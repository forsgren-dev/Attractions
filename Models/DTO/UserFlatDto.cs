using Newtonsoft.Json;

namespace Models.DTO;

public class UserFlatDto : IUser
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    [JsonIgnore]
    public List<IComment> Comments { get; set; }
}
