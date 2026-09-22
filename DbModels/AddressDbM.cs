using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Index(nameof(Seeded))]
[Index(nameof(Street), nameof(PostalCode), nameof(CityDbMCityId), IsUnique = true)]
[Table("Addresses", Schema = "supusr")]
public class AddressDbM : Address
{

    [Key]
    public override Guid AddressId { get; set; }

    [NotMapped]
    public override ICity City
    {
        get => CityDbM;
        set => throw new NotImplementedException();
    }

    [NotMapped]
    public override ICountry Country
    {
        get => CityDbM.CountryDbM;
        set => throw new NotImplementedException();
    }
   #region foreign key
    [JsonIgnore]
    public Guid CityDbMCityId { get; set; }

   [Required]
    [ForeignKey(nameof(CityDbMCityId))]
    public CityDbM CityDbM { get; set; }

    #endregion


    #region constructor
    public AddressDbM() { }

    #endregion

}






