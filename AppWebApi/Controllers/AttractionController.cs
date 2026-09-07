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
            var result = await _service.ListAsync(pageSize, pageNumber);

            return Ok(result);
        }

        [HttpGet]
        [ActionName("SeedAttractions")]
        public async Task<IActionResult> Seed(int nrItems = 10)
        {
            await _service.SeedAsync(nrItems);

            return Ok($"Seeded {nrItems} attractions successfully");
        }

        [HttpDelete]
        [ActionName("RemoveAttractions")]
        public async Task<IActionResult> RemoveAll(bool Seeded = true)
        {
            if (!Seeded)
            {
                return BadRequest("Invalid request. Seeded parameter must be true.");
            }
            await _service.RemoveSeededAsync();
            return Ok("Seeded attractions removed successfully");
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
