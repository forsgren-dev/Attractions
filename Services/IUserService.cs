
using Models.DTO;
using Models;

namespace Services;

public interface IUserService
{
      public Task SeedAsync(int nrItems);

      public Task<ResponsePageDto<IUser>> ListAsync(int pageSize, int pageNumber);

}
