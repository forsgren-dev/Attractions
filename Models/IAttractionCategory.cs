namespace Models;


public interface IAttractionCategory
{
    
    public Guid CategoryId { get; set; }

    public CategoryType Category { get; set; }

}

public enum CategoryType
{
    Museum,
    Park,
    Restaurant,
    Historical,
    Religious,
    Viewpoint,
    Amusement,
    Shopping,
}


