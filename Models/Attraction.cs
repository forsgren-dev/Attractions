using Seido.Utilities.SeedGenerator;



namespace Models;

public class Attraction : IAttraction
{

    public virtual Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public virtual List<ICategory> Categories { get; set; } = new();
    
    public virtual IAddress Address { get; set; }

    public virtual List<IComment> Comments { get; set; } = new();

    

}


