using Seido.Utilities.SeedGenerator;



namespace Models;

public class Attraction : IAttraction
{

    public virtual Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public virtual List<IAttractionCategory> Categories { get; set; }
    
    public virtual IAddress Address { get; set; }

    public virtual List<IComment> Comments { get; set; }

    

}


