using Microsoft.AspNetCore.Mvc;

using Services;
using Models.DTO;

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller
    {
        readonly ILogger<CategoryController> _logger;

        readonly ICategoryService _service;

        [HttpGet]
        [ActionName("ReadCategories")]
        [ProducesResponseType(typeof(ResponsePageDto<CategoryDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> ReadCategories(
            int pageSize = 10,
            int pageNumber = 0,
            string categoryName = null)
        {
            try
            {
                var result = await _service.ReadCategoriesAsync(pageSize, pageNumber, categoryName);

                _logger.LogInformation($"{nameof(ReadCategories)} succeeded. PageSize: {pageSize}, PageNumber: {pageNumber}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(ReadCategories)} failed: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        public CategoryController(
            ILogger<CategoryController> logger,
            ICategoryService service)
        {
            _logger = logger;
            _service = service;
        }
    }
}
