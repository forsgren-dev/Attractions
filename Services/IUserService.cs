
using Models.DTO;
using Models;

namespace Services;

public interface IUserService
{
      public Task SeedAsync(int nrItems);

      public Task<ResponsePageDto<IUser>> ListUsersAsync(int pageSize, int pageNumber, bool flat);
      public Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id);

}
