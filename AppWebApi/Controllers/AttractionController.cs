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
        [ActionName("ListAllAttractions")]
        [ProducesResponseType(typeof(ResponsePageDto<AttractionDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ListAll(int pageNumber = 0, int pageSize = 10)
        {
            try
            {
                var result = await _service.ListAsync(pageSize, pageNumber);

                _logger.LogInformation($"{nameof(ListAll)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ListAll)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }


        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<AttractionDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(Guid id)
        {
            try
            {
                _logger.LogInformation($"{nameof(ReadItem)}: {id}");
                var resp = await _service.ReadAttractionAsync(id);

                if (resp.Item is null)
                    return NotFound($"No attraction found with id {id}");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItem)}: {ex.Message}");
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
