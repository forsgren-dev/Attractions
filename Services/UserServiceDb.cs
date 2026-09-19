using Microsoft.Extensions.Logging;
using DbRepos;
using Models;
using Models.DTO;

namespace Services;

public class UserServiceDb : IUserService
{

      private readonly UserDbRepos _repo = null;
      private readonly ILogger<UserServiceDb> _logger = null;
      public Task SeedAsync(int nrItems) => _repo.SeedAsync(nrItems);

      public Task<ResponsePageDto<UserDto>> ListUsersAsync(int pageSize, int pageNumber, bool flat) => _repo.ReadAllAsync(pageSize, pageNumber, flat);

    public Task<ResponseItemDto<UserDto>> ReadUserAsync(Guid id) => _repo.ReadUserAsync(id);
    public Task<ResponseItemDto<UserDto>> CreateUserAsync(UserCreateDto item) => _repo.CreateUserAsync(item);

    #region constructors
    public UserServiceDb(
        UserDbRepos repo, 
        ILogger<UserServiceDb> logger)
    {
        _repo = repo;
        _logger = logger;
    }   
    
    #endregion
}
