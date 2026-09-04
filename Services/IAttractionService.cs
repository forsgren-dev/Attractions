namespace Services;

public interface IAttractionService
{
    public Task SeedAsync(int nrItems, string[] countries = null);
}
