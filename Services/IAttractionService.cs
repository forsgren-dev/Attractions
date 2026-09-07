using Models;

namespace Services;

public interface IAttractionService
{
    public Task SeedAsync(int nrItems);
    public Task RemoveSeededAsync();
    public Task<List<AttractionDTO>> ListAsync();
}
