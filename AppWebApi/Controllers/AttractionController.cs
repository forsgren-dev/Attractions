using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Services;
using Configuration;
using Configuration.Options;
using Microsoft.Extensions.Options;
using Models;
using Models.DTO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AttractionController : Controller
    {
        readonly ILogger<AttractionController> _logger;

        readonly IAttractionService _service;

        //GET: api/attraction/list
        [HttpGet]
        [ActionName("ListAttractions")]
        [ProducesResponseType(typeof(ResponsePageDto<AttractionDto>), 200)]
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

        [HttpDelete]
        [ActionName("RemoveAttractions")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> RemoveAll(bool Seeded = true)
        {
            try
            {
                if (!Seeded)
                {
                    const string message = "Invalid request. Only seeded attractions can be removed.";
                    _logger.LogError($"{nameof(RemoveAll)} failed: {message}");
                    return BadRequest(message);
                }

                await _service.RemoveSeededAsync();
                _logger.LogInformation($"{nameof(RemoveAll)} succeeded.");
                return Ok("Seeded attractions removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(RemoveAll)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }   


        //GET: api/attraction/seed?nrItems=10

        public AttractionController(
            ILogger<AttractionController> logger,
            IAttractionService service)
        {
            _logger = logger;
            _service = service;
        }


    }

}
