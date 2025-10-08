using Microsoft.AspNetCore.Mvc;
using GetChildById.Services;

namespace GetChildById.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetChildByIdController : ControllerBase
    {
        private readonly GetChildByIdService _service;

        public GetChildByIdController(GetChildByIdService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var child = await _service.GetChildByIdAsync(id);
            return child is not null ? Ok(child) : NotFound();
        }
    }
}
