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
    public class AttractionController : Controller
    {
        readonly ILogger<AttractionController> _logger;

        readonly IAttractionService _service;

        //GET: api/attraction/list
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var result = 10; // await _service.ListAsync();
            return Ok(result);
        }

        //GET: api/attraction/seed?nrItems=10&countries=Sweden,Norway
        [HttpGet]
        public async Task<IActionResult> Seed(int nrItems = 10, string countries = "Sweden,Norway,Denmark,Finland")
        {
            var countryNames = countries
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            await _service.SeedAsync(nrItems, countryNames);

            return Ok($"Seeded {nrItems} attractions successfully");
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
