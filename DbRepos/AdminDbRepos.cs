using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

using DbContext;
using Configuration;
using Models.DTO;

namespace DbRepos;

public class AdminDbRepos
{
    private readonly ILogger<AdminDbRepos> _logger;
    private Encryptions _encryptions;
    private readonly MainDbContext _dbContext;
    private readonly AttractionDbRepos _attractionDbRepos;
    private readonly UserDbRepos _userDbRepos;
    private readonly CommentDbRepos _commentDbRepos;

    public async Task SeedAsync(int nrItems)
    {
        await _attractionDbRepos.SeedAsync(nrItems);
        await _userDbRepos.SeedAsync(nrItems);
        await _commentDbRepos.SeedAttractionCommentsAsync();
    }

    public async Task RemoveSeededAsync()
    {
        var seededAttractions = await _dbContext.Attractions.Where(a => a.Seeded == true).ToListAsync();

        _dbContext.Attractions.RemoveRange(seededAttractions);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ResponseItemDto<DbInfoDto>> GuestInfoAsync() => await DbInfo();


    private async Task<ResponseItemDto<DbInfoDto>> DbInfo()
    {
        var info = await _dbContext.DbInfoView.FirstAsync();

        return new ResponseItemDto<DbInfoDto>
        {
#if DEBUG
            ConnectionString = _dbContext.dbConnection,
#endif

            Item = info
        };
    }

    public AdminDbRepos(
        ILogger<AdminDbRepos> logger,
        Encryptions encryptions,
        MainDbContext context,
        AttractionDbRepos attractionDbRepos,
        UserDbRepos userDbRepos,
        CommentDbRepos commentDbRepos)
    {
        _logger = logger;
        _encryptions = encryptions;
        _dbContext = context;
        _attractionDbRepos = attractionDbRepos;
        _userDbRepos = userDbRepos;
        _commentDbRepos = commentDbRepos;
    }
}
