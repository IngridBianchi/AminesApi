using Microsoft.AspNetCore.Http;

namespace BlobUpload.Models
{
    public class BlobUploadRequest
    {
        public IFormFile File { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
    }
}
