using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace Models.DTO;

public class AttractionDto
{
    public Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public  AttractionAddressDto Address { get; set; }
    public List<string> Categories { get; set; } = new();

    //Hittade denna metod för att dölja sjävla '"comments": []' i json-svaret när kommentarer inte ska visas
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<CommentDto> Comments { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public ResponsePageDto<CommentDto> CommentsPage { get; set; }

}
