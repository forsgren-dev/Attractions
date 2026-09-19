
using Models.DTO;
using Models;

namespace Services;

public interface IUserService
{
      public Task SeedAsync(int nrItems);

      public Task<ResponsePageDto<UserDto>> ListUsersAsync(int pageSize = 10, int pageNumber = 0, bool flat = false);
      public Task<ResponseItemDto<UserDto>> ReadUserAsync(Guid id);
      public Task<ResponseItemDto<UserDto>> CreateUserAsync(UserCreateDto item);

}
