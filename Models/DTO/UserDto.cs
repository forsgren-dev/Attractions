using Newtonsoft.Json;

namespace Models.DTO;

public class UserDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<UserCommentsDto> Comments { get; set; }
}
