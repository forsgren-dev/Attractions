using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;
using Microsoft.EntityFrameworkCore;

namespace DbModels;

public class CommentDbM : Comment
{
    [Key]
    public override Guid CommentId { get; set; }

    [NotMapped]
    public override IAttraction Attraction
    {
        get => AttractionDbM;
        set => AttractionDbM = (AttractionDbM)value;
    }

    public Guid AttractionId { get; set; }

    public AttractionDbM AttractionDbM { get; set; }
}
   

    



