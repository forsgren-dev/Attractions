using Seido.Utilities.SeedGenerator;

namespace Models;

public class Country : ICountry, ISeed<Country>
{

    public virtual Guid CountryId { get; set; }
    public string CountryName { get; set; }

    public virtual List<ICity> Cities { get; set; } = new();

    public bool Seeded { get; set; } = false;

    public virtual Country Seed(SeedGenerator seeder)
    {
        CountryId = Guid.NewGuid();
        CountryName = seeder.Country;
        Seeded = true;
        return this;
    }

}


