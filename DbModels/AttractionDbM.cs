using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Attractions", Schema = "supusr")]
public class AttractionDbM : Attraction
{

    [Key]
    public override Guid AttractionId { get; set; }

    [NotMapped]
    public override List<IComment> Comments
    {
        get => CommentDbM.Cast<IComment>().ToList();
        set => throw new NotImplementedException();
    }

    public List<CommentDbM> CommentDbM { get; set; } = new();

    [NotMapped]
    public override IAddress Address
    {
        get => AddressDbM;
        set => throw new NotImplementedException();
    }

    public Guid AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public AddressDbM AddressDbM { get; set; }

    #region constructor
    public AttractionDbM() { }

    #endregion

}






