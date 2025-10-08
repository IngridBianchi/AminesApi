using Microsoft.AspNetCore.Mvc;
using BlobUpload.Models;
using SharedLibrary.Data;
using System.IO;
using System.Threading.Tasks;

namespace BlobUpload.Controllers;

[ApiController]
[Route("Add/Blob")]
public class BlobController : ControllerBase
{
    private readonly IBlob _blob;

    public BlobController(IBlob blob)
    {
        _blob = blob;
    }

    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] BlobUploadRequest request)
    {
        if (request.File == null || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Lastname))
            return BadRequest("Faltan datos");

        var fileName = $"{request.Name.ToLower()}{request.Lastname.ToLower()}{Path.GetExtension(request.File.FileName)}";
        await _blob.Upload(request.File, fileName);
        return Ok(new { message = "Archivo subido correctamente", fileName });
    }
}
