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

    public Guid AttractionId { get; set; }

    [ForeignKey(nameof(AttractionId))]
    public AttractionDbM AttractionDbM { get; set; }
}






