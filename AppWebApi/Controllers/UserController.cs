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
        [ActionName("ListUsers")]
        [ProducesResponseType(typeof(ResponsePageDto<IUser>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> List(int pageSize = 10, int pageNumber = 0)
        {
            try
            {
                var result = await _service.ListAsync(pageSize, pageNumber);

                _logger.LogInformation($"{nameof(List)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(List)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }



        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<IUser>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(Guid id)
        {
            try
            {
                _logger.LogInformation($"{nameof(ReadItem)}: {id}");
                var resp = await _service.ReadUserAsync(id);

                if (resp.Item is null)
                    return NotFound($"No user found with id {id}");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItem)}: {ex.Message}");
                return BadRequest(ex.Message);
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