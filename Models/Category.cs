using Seido.Utilities.SeedGenerator;

namespace Models;

public class Category : ICategory
{

    public virtual Guid CategoryId { get; set; }

    public CategoryType CategoryType { get; set; }
}
