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

        //GET: api/attraction/list
        [HttpGet]
        [ProducesResponseType(typeof(ResponsePageDto<CommentDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ReadCommentsByAttractionId(
            Guid AttractionId,
            int pageSize = 10,
            int pageNumber = 0)
        {
            try
            {
                var result = await _service.ReadCommentsByAttractionIdAsync(AttractionId, pageSize, pageNumber);

                _logger.LogInformation($"{nameof(ReadCommentsByAttractionId)} succeeded. AttractionId: {AttractionId}, PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadCommentsByAttractionId)} failed. AttractionId: {AttractionId}, PageSize: {pageSize}, PageNumber: {pageNumber}, Error: {ex.Message}");
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
