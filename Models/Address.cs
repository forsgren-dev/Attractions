using Seido.Utilities.SeedGenerator;

namespace Models;

public class Address : IAddress, IEquatable<Address>, ISeed<Address>
{

    public virtual Guid AddressId { get; set; }

    public string Street { get; set; }
    public string PostalCode { get; set; }
    public virtual ICity City { get; set; }
    public virtual ICountry Country { get; set; }
    public bool Seeded { get; set; } = false;

    public Address Seed(SeedGenerator seeder)
    {
        var countryName = seeder.Country;

        AddressId = Guid.NewGuid();
        Street = seeder.StreetAddress(countryName);
        PostalCode = seeder.ZipCode.ToString();

        Country = new Country
        {
            CountryId = Guid.NewGuid(),
            CountryName = countryName
        };

        City = new City
        {
            CityId = Guid.NewGuid(),
            CityName = seeder.City(countryName),
            Country = Country
        };

        Seeded = true;
        return this;
    }


    public bool Equals(Address other)
    {
        if (other is null) return false;

        return string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase)
            && string.Equals(PostalCode, other.PostalCode, StringComparison.OrdinalIgnoreCase)
            && City?.CityId == other.City?.CityId
            && Country?.CountryId == other.Country?.CountryId;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Address);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Street?.ToLowerInvariant(),
            PostalCode?.ToLowerInvariant(),
            City?.CityId,
            Country?.CountryId);

    }


}


