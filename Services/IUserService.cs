
using Models.DTO;
using Models;

namespace Services;

public interface IUserService
{
      public Task SeedAsync(int nrItems);

      public Task<ResponsePageDto<UserDto>> ReadUsersAsync(
            int pageSize = 10,
            int pageNumber = 0,
            string userName = null,
            bool showComments = false);
      public Task<ResponseItemDto<UserDto>> ReadUserAsync(Guid id);
      public Task<ResponseItemDto<UserDto>> CreateUserAsync(UserCreateDto item);
      public Task<ResponseItemDto<UserDto>> DeleteUserAsync(Guid id);

}
