using Seido.Utilities.SeedGenerator;

namespace Models;

public class AttractionCategory : IAttractionCategory
{

    public virtual Guid CategoryId { get; set; }

    public  CategoryType Category { get; set; }
}


