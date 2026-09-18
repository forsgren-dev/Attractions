
using Models.DTO;
namespace Services;

public interface IAdminService
{
    public Task<ResponseItemDto<DbInfoDto>> GuestInfoAsync();
    public Task SeedAsync(int nrItems);
    public Task RemoveSeededAsync();
}
