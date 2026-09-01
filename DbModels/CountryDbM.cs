using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

public class CountryDbM : Country
{
    
 [Key]
    public override Guid CountryId { get; set; }

}
   

    



