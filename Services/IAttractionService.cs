using Models;

namespace Services;

public interface IAttractionService
{
    public Task SeedAsync(int nrItems);
    public Task<List<AttractionListItem>> ListAsync();
}
