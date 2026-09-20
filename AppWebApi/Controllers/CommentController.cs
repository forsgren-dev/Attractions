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

        public CommentController(
                           ILogger<CommentController> logger,
                           ICommentService service)
        {
            _logger = logger;
            _service = service;
        }

    }

}
