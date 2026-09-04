using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Categories", Schema = "supusr")]
public class CategoryDbM : AttractionCategory
{
    
 [Key]
    public override Guid CategoryId { get; set; }
    
    public List<AttractionDbM> AttractionDbM { get; set; } = new();

}
   

    



