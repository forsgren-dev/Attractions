using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

using Seido.Utilities.SeedGenerator;
using Models;
using Models.DTO;

namespace DbModels;

[Index(nameof(UserName), IsUnique = true)]
[Index(nameof(Seeded))]
[Table("Users", Schema = "supusr")]
public class UserDbM : User, ISeed<UserDbM>
{
    
 [Key]
    public override Guid UserId { get; set; }

    [NotMapped]
    public override List<IComment> Comments
    {
        get => CommentDbM.Cast<IComment>().ToList();
        set => throw new NotImplementedException();
    }

    public List<CommentDbM> CommentDbM { get; set; } = new();

    public UserDbM() { }

    public UserDbM(UserCreateDto itemDto)
    {
        UserId = Guid.NewGuid();
        UserName = itemDto.UserName;
        Seeded = false;
    }

    public override UserDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }
}
   

    



