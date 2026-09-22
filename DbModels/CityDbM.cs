using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Index(nameof(CityName))]
[Index(nameof(Seeded))]
[Table("Cities", Schema = "supusr")]
public class CityDbM : City, IEquatable<CityDbM>, ISeed<CityDbM>
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

    public override CityDbM Seed(SeedGenerator seeder)
    {
        CityId = Guid.NewGuid();
        CountryDbM = new CountryDbM().Seed(seeder);
        CityName = seeder.City(CountryDbM.CountryName);
        Seeded = true;
        return this;
    }

    public bool Equals(CityDbM other) =>
        other is not null && Seeded == other.Seeded
        && StringComparer.OrdinalIgnoreCase.Equals(CityName, other.CityName)
        && StringComparer.OrdinalIgnoreCase.Equals(
            CountryDbM?.CountryName,
            other?.CountryDbM?.CountryName);

    public override bool Equals(object obj) =>
        Equals(obj as CityDbM);

    public override int GetHashCode() =>
        HashCode.Combine(
            Seeded,
            StringComparer.OrdinalIgnoreCase.GetHashCode(CityName ?? string.Empty),
            StringComparer.OrdinalIgnoreCase.GetHashCode(CountryDbM?.CountryName ?? string.Empty));
    

}
   

    



