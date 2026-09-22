namespace Models.DTO;

public class AttractionUpdateDto
{
    public Guid? AttractionId { get; set; }
    public string AttractionName { get; set; }
    public string AttractionDescription { get; set; }
    public string Street { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public List<Guid?> CategoriesId { get; set; } = null;
    

    public void EnsureValidity()
    {
        if (AttractionId is null)
        {
            throw new ArgumentException($"{nameof(AttractionId)} cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(AttractionName))
        {
            throw new ArgumentException($"{nameof(AttractionName)} cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(AttractionDescription))
        {
            throw new ArgumentException($"{nameof(AttractionDescription)} cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(City))
        {
            throw new ArgumentException($"{nameof(City)} cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(Country))
        {
            throw new ArgumentException($"{nameof(Country)} cannot be empty.");
        }
    }
}
