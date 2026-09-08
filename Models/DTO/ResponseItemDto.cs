namespace Models.DTO;

public class ResponseItemDto<T>
{
    #if DEBUG
    public string ConnectionString { get; set; }
    #endif
    
    public T Item { get; set; }
}

