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

      public Task<ResponsePageDto<IUser>> ListAsync(int pageSize, int pageNumber) => _repo.ListAsync(pageSize, pageNumber);

    public Task<ResponseItemDto<IUser>> ReadUserAsync(Guid id) => _repo.ReadUserAsync(id);

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
