namespace Models.DTO;

public class AttractionDto 
{
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public  AttractionAddressDto Address { get; set; }
}


public class AttractionAddressDto 
{
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }

}