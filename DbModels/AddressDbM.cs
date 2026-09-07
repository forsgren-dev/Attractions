using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

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
    public CityDbM CityDbM { get; set; }

    #endregion


    #region constructor
    public AddressDbM() { }

    #endregion

}






