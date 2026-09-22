using Seido.Utilities.SeedGenerator;

namespace Models;

public class City : ICity, ISeed<City>
{

    public virtual Guid CityId { get; set; }
    public string CityName { get; set; }

    public virtual ICountry Country { get; set; }

    public virtual List<IAddress> Addresses { get; set; }

    public bool Seeded { get; set; } = false;

    public virtual City Seed(SeedGenerator seeder)
    {
        var country = new Country().Seed(seeder);
        CityId = Guid.NewGuid();
        CityName = seeder.City(country.CountryName);
        Country = country;
        Seeded = true;
        return this;
    }

}


