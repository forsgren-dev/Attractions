namespace Models;


public interface ICategory
{
    
    public Guid CategoryId { get; set; }

    public CategoryType CategoryType { get; set; }

}

public enum CategoryType
{
    Museum,
    Park,
    Restaurant,
    Historical,
    Religious,
    Art,
    Architecture,
    Nature,
    Amusement,
    Shopping,
}


