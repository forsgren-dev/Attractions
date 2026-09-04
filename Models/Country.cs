using Seido.Utilities.SeedGenerator;

namespace Models;

public class Country : ICountry
{

    public virtual Guid CountryId { get; set; }
    public string CountryName { get; set; }

    public virtual List<ICity> Cities { get; set; } = new();

}


