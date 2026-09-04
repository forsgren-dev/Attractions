using Seido.Utilities.SeedGenerator;



namespace Models;

public class Attraction : IAttraction, ISeed<Attraction>
{

    public virtual Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public virtual List<ICategory> Categories { get; set; } = new();

    public virtual IAddress Address { get; set; }

    public virtual List<IComment> Comments { get; set; } = new();

    #region seeding
    public bool Seeded { get; set; } = false;

    public Attraction Seed(SeedGenerator seeder)
    {
        AttractionId = Guid.NewGuid();
        AttractionName = seeder.AttractionName;
        AttractionDescription = seeder.LatinSentence;
        Seeded = true;
        return this;
    }
    #endregion
}


