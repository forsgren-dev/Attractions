using Seido.Utilities.SeedGenerator;

namespace Models;

public class User : IUser
{

    public virtual Guid UserId { get; set; }
    public string UserName { get; set; }

    public virtual List<IComment> Comments { get; set; }
    

}


