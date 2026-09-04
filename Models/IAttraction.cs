namespace Models;


public interface IAttraction
{
    public Guid AttractionId { get; set; }
    public string AttractionName { get; set; }
     public IAddress Address { get; set; }

    public List<IComment> Comments { get; set; }

}


