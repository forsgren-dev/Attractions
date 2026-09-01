using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;

using Seido.Utilities.SeedGenerator;
using DbModels;
using DbContext;
using Configuration;

namespace DbRepos;

public class AttractionDbRepos
{
    private const string _seedSource = "./app-seeds.json";
    private readonly ILogger<AttractionDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;

    // public async Task SeedAsync(int nrItems)
    // {
    //     //Create a seeder
    //     var fn = Path.GetFullPath(_seedSource);
    //     var seeder = new SeedGenerator(fn);

    //     var creditcards = seeder.ItemsToList<CreditCardDbM>(nrItems);
    //     _dbContext.CreditCards.AddRange(creditcards);

    //     //Save changes to the database
    //     await _dbContext.SaveChangesAsync();
    // }

    public AttractionDbRepos(
        ILogger<AttractionDbRepos> logger,
        Encryptions encryptions,
        MainDbContext context)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
    }
}
