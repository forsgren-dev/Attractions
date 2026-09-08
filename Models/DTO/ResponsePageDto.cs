namespace Models.DTO;

public class ResponsePageDto<T>
{
    #if DEBUG
    public string ConnectionString { get; set; }
    #endif
    
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int DbItemsCount { get; set; }
    public List<T> Items { get; set; } = new();
}

