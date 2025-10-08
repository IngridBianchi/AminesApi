using Microsoft.AspNetCore.Mvc;
using GetChildren.Services;

namespace GetChildren.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChildrenController : ControllerBase
    {
        private readonly IChildService _service;

        public ChildrenController(IChildService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var children = await _service.GetAllChildrenAsync();
            return Ok(children);
        }
    }
}
