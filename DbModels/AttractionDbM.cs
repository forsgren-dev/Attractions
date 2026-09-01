using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

public class AttractionDbM : Attraction
{

    [Key]
    public override Guid AttractionId { get; set; }

    [NotMapped]
    public override List<IComment> Comments
    {
        get => CommentDbM.Cast<IComment>().ToList();
        set => CommentDbM = value.Cast<CommentDbM>().ToList();
    }

    public List<CommentDbM> CommentDbM { get; set; } = new();

    #region constructor
    public AttractionDbM() { }

    #endregion

}






