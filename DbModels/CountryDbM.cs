using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

using Seido.Utilities.SeedGenerator;
using Models;

namespace DbModels;

[Index(nameof(CountryName), nameof(Seeded), IsUnique = true)]
[Index(nameof(Seeded))]
[Table("Countries", Schema = "supusr")]
public class CountryDbM : Country, IEquatable<CountryDbM>, ISeed<CountryDbM>
{
    
 [Key]
    public override Guid CountryId { get; set; }

    [NotMapped]
    public override List<ICity> Cities
    {
        get => CityDbM.Cast<ICity>().ToList();
        set => throw new NotImplementedException();
    }

    public List<CityDbM> CityDbM { get; set; } = new();

    public override CountryDbM Seed(SeedGenerator seeder)
    {
        base.Seed(seeder);
        return this;
    }

    public bool Equals(CountryDbM other) =>
        other is not null && Seeded == other.Seeded
        && StringComparer.OrdinalIgnoreCase.Equals(CountryName, other.CountryName);

    public override bool Equals(object obj) =>
        Equals(obj as CountryDbM);

    public override int GetHashCode() =>
        HashCode.Combine(Seeded, StringComparer.OrdinalIgnoreCase.GetHashCode(CountryName ?? string.Empty));

}
   

    



