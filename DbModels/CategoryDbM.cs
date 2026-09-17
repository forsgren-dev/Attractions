using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Categories", Schema = "supusr")]
public class CategoryDbM : Category, IEquatable<CategoryDbM>
{
    
 [Key]
    public override Guid CategoryId { get; set; }

    public string CategoryName
    {
        get => CategoryType.ToString();
        set { }
    }
    
    public List<AttractionDbM> AttractionDbM { get; set; } = new();

    public bool Equals(CategoryDbM other) =>
        CategoryType == other?.CategoryType;

    public override bool Equals(object obj) =>
        Equals(obj as CategoryDbM);

    public override int GetHashCode() =>
        CategoryType.GetHashCode();

}
   

    



