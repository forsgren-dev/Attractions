using System.Text.Json.Serialization;

namespace Models.DTO;

public class AttractionDto
{
    public Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public  AttractionAddressDto Address { get; set; }
    public List<string> Categories { get; set; } = new();



}
