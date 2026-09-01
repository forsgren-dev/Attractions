using Seido.Utilities.SeedGenerator;

namespace Models;

public class Attraction : IAttraction
{

    public virtual Guid AttractionId { get; set; }
    public string AttractionName { get; set; }

}


