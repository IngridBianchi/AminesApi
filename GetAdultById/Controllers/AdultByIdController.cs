using Microsoft.AspNetCore.Mvc;
using GetAdultById.Services;

namespace GetAdultById.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdultByIdController : ControllerBase
    {
        private readonly AdultByIdService _service;

        public AdultByIdController(AdultByIdService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var adult = await _service.GetAdultByIdAsync(id);
            return adult is not null ? Ok(adult) : NotFound();
        }
    }
}
