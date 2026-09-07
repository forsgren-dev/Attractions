namespace Models;

public class AttractionDTO : IAttractionDTO
{
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public IAttractionAddressDTO Address { get; set; }
}

public class AttractionAddressDTO : IAttractionAddressDTO
{
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}
