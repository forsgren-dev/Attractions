using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Attractions", Schema = "supusr")]
public class AttractionDbM : Attraction, ISeed<AttractionDbM>
{
    [Key]
    public override Guid AttractionId { get; set; }

     [NotMapped]
     public override List<ICategory> Categories

    {
    
        get => CategoryDbM.Cast<ICategory>().ToList();
        set => throw new NotImplementedException();
    }
    public List<CategoryDbM> CategoryDbM { get; set; } = new();


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

    // public Guid AddressId { get; set; }

    #region foreign key
    public AddressDbM AddressDbM { get; set; }
    
    #endregion

    #region constructor
    public AttractionDbM() { }

    #endregion

    #region seeding
    
    public override AttractionDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        Seeded = true;
        return this;
    }

    #endregion

}






