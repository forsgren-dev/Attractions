using Seido.Utilities.SeedGenerator;

namespace Models;

public class Address : IAddress
{

    public virtual Guid AddressId { get; set; }

    public string Street { get; set; }
    public string PostalCode { get; set; }
    public virtual ICity City { get; set; }
    public virtual ICountry Country { get; set; }

}


