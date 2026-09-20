using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Services;
using Configuration;
using Configuration.Options;
using Models.DTO;
using Models;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        readonly ILogger<UserController> _logger;

        readonly IUserService _service;


        [HttpGet]
        [ActionName("ReadUsers")]
        [ProducesResponseType(typeof(ResponsePageDto<UserDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ReadUsers(
            int pageSize = 10,
            int pageNumber = 0,
            string userName = null,
            bool showComments = false)
        {
            try
            {
                var result = await _service.ReadUsersAsync(pageSize, pageNumber, userName, showComments);

                _logger.LogInformation($"{nameof(ReadUsers)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadUsers)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }


        [HttpGet()]
        [ActionName("ReadUserById")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<UserDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadUserById(Guid id)
        {
            try
            {
                _logger.LogInformation($"{nameof(ReadUserById)}: {id}");
                var resp = await _service.ReadUserAsync(id);

                if (resp.Item is null)
                    return NotFound($"No user found with id {id}");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadUserById)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost()]
        [ActionName("CreateUser")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<UserDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto item)
        {
            try
            {
                item.EnsureValidity();
                _logger.LogInformation($"{nameof(CreateUser)}:");

                var resp = await _service.CreateUserAsync(item);
                _logger.LogInformation($"item {resp.Item.UserId} created");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateUser)}: {ex.Message}");
                return BadRequest($"Could not create. Error {ex.Message}");
            }
        }

        public UserController(
                   ILogger<UserController> logger,
                   IUserService service)
        {
            _logger = logger;
            _service = service;
        }


    }



}
