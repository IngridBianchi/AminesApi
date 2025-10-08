using Microsoft.AspNetCore.Mvc;
using GetAdults.Services;

namespace GetAdults.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdultsController : ControllerBase
    {
        private readonly IAdultService _service;

        public AdultsController(IAdultService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var adults = await _service.GetAllAdultsAsync();
            return Ok(adults);
        }
    }
}
