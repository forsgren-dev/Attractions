namespace Models;


public interface IAddress
{
    public Guid AddressId { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public ICity City { get; set; }
    public ICountry Country { get; set; }

}


