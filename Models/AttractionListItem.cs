namespace Models;

public class AttractionListItem : IAttractionListItem
{
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public IAttractionAddressItem Address { get; set; }
}

public class AttractionAddressItem : IAttractionAddressItem
{
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}
