using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Cities", Schema = "supusr")]
public class CityDbM : City
{
    
 [Key]
    public override Guid CityId { get; set; }

    [NotMapped]
    public override List<IAddress> Addresses
    {
        get => AddressDbM.Cast<IAddress>().ToList();
        set => throw new NotImplementedException();
    }

    public List<AddressDbM> AddressDbM { get; set; } = new();
    

}
   

    



