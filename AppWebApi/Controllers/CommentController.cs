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
    public class CommentController : Controller
    {
        readonly ILogger<CommentController> _logger;

        readonly ICommentService _service;

        [HttpGet]
        [ActionName("ReadComments")]
        [ProducesResponseType(typeof(ResponsePageDto<CommentDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ReadComments(
            int pageSize = 10,
            int pageNumber = 0,
            Guid? id = null)
        {
            try
            {
                var result = await _service.ReadCommentsAsync(pageSize, pageNumber, id);

                _logger.LogInformation($"{nameof(ReadComments)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}, Id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadComments)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [ActionName("CreateComment")]
        [ProducesResponseType(typeof(ResponseItemDto<CommentDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> CreateComment([FromBody] CommentCreateDto item)
        {
            try
            {
                if (item == null)
                {
                    throw new ArgumentException($"{nameof(item)} cannot be null.");
                }

                item.EnsureValidity();
                _logger.LogInformation($"{nameof(CreateComment)}:");

                var result = await _service.CreateCommentAsync(item);

                _logger.LogInformation($"item {result.Item.CommentId} created");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(CreateComment)}: {ex.Message}");
                return BadRequest($"Could not create. Error {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteItem")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<CommentDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                var idArg = Guid.Parse(id);

                _logger.LogInformation($"{nameof(DeleteItem)}: {nameof(idArg)}: {idArg}");

                var item = await _service.DeleteCommentAsync(idArg);

                _logger.LogInformation($"item {idArg} deleted");
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(DeleteItem)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        public CommentController(
                           ILogger<CommentController> logger,
                           ICommentService service)
        {
            _logger = logger;
            _service = service;
        }

    }

}
