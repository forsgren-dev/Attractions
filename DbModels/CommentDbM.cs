using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DbModels;

[Table("Comments", Schema = "supusr")]
public class CommentDbM : Comment
{
    [Key]
    public override Guid CommentId { get; set; }

    [NotMapped]
    public override IAttraction Attraction
    {
        get => AttractionDbM;
        set => throw new NotImplementedException();
    }
    #region foreign key
    public AttractionDbM AttractionDbM { get; set; }
    #endregion

    [NotMapped]
    public override IUser User
    {
        get => UserDbM;
        set => throw new NotImplementedException();
    }
   #region foreign key
    public UserDbM UserDbM { get; set; }

    #endregion
}







