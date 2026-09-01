using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Services;
using Configuration;
using Configuration.Options;
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
        public async Task<IActionResult> List()
        {
            var result = 10; // await _service.ListAsync();
            return Ok(result);
        }
    }

}