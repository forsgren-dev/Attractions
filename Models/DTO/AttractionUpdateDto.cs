namespace Models.DTO;

public class AttractionUpdateDto 
{
    public Guid? AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public  Guid? AddressId { get; set; } = null;
    public List<Guid> CategoriesId { get; set; } = null;
}
