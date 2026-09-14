using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Table("Cities", Schema = "supusr")]
public class CityDbM : City, IEquatable<CityDbM>
{
    
    [Key]
    public override Guid CityId { get; set; }

    [NotMapped]
    public override ICountry Country
    {
        get => CountryDbM;
        set => throw new NotImplementedException();
    }
    #region foreign key
    [Required]
    public CountryDbM CountryDbM { get; set; }
    
    #endregion

    [NotMapped]
    public override List<IAddress> Addresses
    {
        get => AddressDbM.Cast<IAddress>().ToList();
        set => throw new NotImplementedException();
    }

    public List<AddressDbM> AddressDbM { get; set; } = new();

    public bool Equals(CityDbM other) =>
        StringComparer.OrdinalIgnoreCase.Equals(CityName, other?.CityName)
        && StringComparer.OrdinalIgnoreCase.Equals(
            CountryDbM?.CountryName,
            other?.CountryDbM?.CountryName);

    public override bool Equals(object obj) =>
        Equals(obj as CityDbM);

    public override int GetHashCode() =>
        HashCode.Combine(
            StringComparer.OrdinalIgnoreCase.GetHashCode(CityName ?? string.Empty),
            StringComparer.OrdinalIgnoreCase.GetHashCode(CountryDbM?.CountryName ?? string.Empty));
    

}
   

    



