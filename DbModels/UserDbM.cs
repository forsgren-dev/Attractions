using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Users", Schema = "supusr")]
public class UserDbM : User
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
    

}
   

    



