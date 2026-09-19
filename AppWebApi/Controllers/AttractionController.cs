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

        //GET: api/attraction/readallattractions
        [HttpGet]
        [ActionName("ReadAttractions")]
        [ProducesResponseType(typeof(ResponsePageDto<AttractionDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ReadAttractions(
            int pageNumber = 0,
            int pageSize = 10,
            string attractionName = null,
            string category = null,
            string description = null,
            string city = null,
            string country = null)
        {
            try
            {
                var result = await _service.ReadAttractionsAsync(
                    pageSize,
                    pageNumber,
                    attractionName,
                    category,
                    description,
                    city,
                    country);

                _logger.LogInformation($"{nameof(ReadAttractions)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadAttractions)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

          [HttpGet]
        [ActionName("ReadAttractionsNoComments")]
        [ProducesResponseType(typeof(ResponsePageDto<AttractionDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ReadAttractionsNoComments(
            int pageNumber = 0,
            int pageSize = 10,
            string city = null,
            string country = null)
        {
            try
            {
                var result = await _service.ReadAttractionsNoCommentsAsync(
                    pageSize,
                    pageNumber,
                    city,
                    country);

                _logger.LogInformation($"{nameof(ReadAttractionsNoComments)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadAttractionsNoComments)} failed: {ex.Message}");
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
                var resp = await _service.ReadSingleAttractionAsync(id);

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
