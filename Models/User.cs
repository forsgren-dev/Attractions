using Seido.Utilities.SeedGenerator;

namespace Models;

public class User : IUser, ISeed<User>
{

    public virtual Guid UserId { get; set; }
    public string UserName { get; set; }

    public virtual List<IComment> Comments { get; set; }
    public bool Seeded { get; set; } = false;
    public virtual User Seed(SeedGenerator seeder)
    {
        UserId = Guid.NewGuid();
        UserName = $"{seeder.FirstName}{seeder.FirstName}{seeder.Next(10, 9000)}";
        Seeded = true;
        return this;
    }
}


