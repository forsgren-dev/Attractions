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

        //GET: api/attraction/readattractions - Listar attractions med filter och pagination
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
            string country = null,
            bool? hasComments = null,
            bool showComments = false)
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
                    country,
                    hasComments,
                    showComments);

                _logger.LogInformation($"{nameof(ReadAttractions)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadAttractions)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
        //GET: api/attraction/readattractionswithnocomments - Listar attraktioner som saknar kommentarer
        [HttpGet]
        [ActionName("ReadAttractionsWithNoComments")]
        [ProducesResponseType(typeof(ResponsePageDto<AttractionDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ReadAttractionsWithNoComments(
            int pageNumber = 0,
            int pageSize = 10,
            string city = null,
            string country = null)
        {
            try
            {
                var result = await _service.ReadAttractionsWithNoCommentsAsync(
                    pageSize,
                    pageNumber,
                    city,
                    country);

                _logger.LogInformation($"{nameof(ReadAttractionsWithNoComments)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadAttractionsWithNoComments)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadAttractionById")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<AttractionDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadAttractionById(
            Guid id,
            int pageNumber = 0,
            int pageSize = 10,
            bool showComments = true)
        {
            try
            {
                _logger.LogInformation($"{nameof(ReadAttractionById)}: {id}, PageSize: {pageSize}, PageNumber: {pageNumber}");
                var resp = await _service.ReadSingleAttractionAsync(id, pageSize, pageNumber, showComments);

                if (resp.Item is null)
                    return NotFound($"No attraction found with id {id}");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadAttractionById)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadItemDto")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<AttractionUpdateDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(Guid id)
        {
            try
            {
                _logger.LogInformation($"{nameof(ReadItemDto)}: {id}");

                var resp = await _service.ReadAttractionDtoAsync(id);

                if (resp.Item is null)
                    return NotFound($"No attraction found with id {id}");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadItemDto)}: {ex.Message}");
                return BadRequest(ex.Message);
            }

        }

        [HttpPost()]
        [ActionName("CreateAttraction")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<AttractionDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateAttraction([FromBody] AttractionCreateDto item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentException($"{nameof(item)} cannot be null.");
                }

                item.EnsureValidity();
                _logger.LogInformation($"{nameof(CreateAttraction)}:");

                var resp = await _service.CreateAttractionAsync(item);
                _logger.LogInformation($"item {resp.Item.AttractionId} created");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateAttraction)}: {ex.Message}");
                return BadRequest($"Could not create. Error {ex.Message}");
            }
        }

        [HttpPut()]
        [ActionName("UpdateAttraction")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<AttractionDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateAttraction([FromBody] AttractionUpdateDto item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentException($"{nameof(item)} cannot be null.");
                }

                item.EnsureValidity();
                _logger.LogInformation($"{nameof(UpdateAttraction)}: {item.AttractionId}");

                var resp = await _service.UpdateAttractionAsync(item);
                _logger.LogInformation($"item {resp.Item.AttractionId} updated");

                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(UpdateAttraction)}: {ex.Message}");
                return BadRequest($"Could not update. Error {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteItem")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<AttractionDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                var idArg = Guid.Parse(id);

                _logger.LogInformation($"{nameof(DeleteItem)}: {nameof(idArg)}: {idArg}");

                var item = await _service.DeleteAttractionAsync(idArg);

                _logger.LogInformation($"item {idArg} deleted");
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteItem)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }


        public AttractionController(
            ILogger<AttractionController> logger,
            IAttractionService service)
        {
            _logger = logger;
            _service = service;
        }


    }

}
