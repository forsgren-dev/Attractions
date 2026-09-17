using Newtonsoft.Json;
namespace Models.DTO;

public class UserCuDto
{
    public Guid? UserId { get; set; }
    public string UserName { get; set; }
    public List<Guid?> CommentsId { get; set; } = null;

}
