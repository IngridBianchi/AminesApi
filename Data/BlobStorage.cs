using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oci.Common;
using Oci.ObjectstorageService;
using Oci.ObjectstorageService.Models;
using Oci.ObjectstorageService.Requests;
using Oci.ObjectstorageService.Responses;
using System;
using System.IO;
using System.Threading.Tasks;
using Oci.Common.Auth;

namespace AminesApi.Data
{
    public interface IBlob
    {
        Task Upload(IFormFile file, string fileName);
    }

    public class BlobStorage : IBlob
    {
        private readonly ObjectStorageClient _client;
        private readonly string? _bucketName;
        private readonly string? _namespace;
        private readonly ILogger<BlobStorage> _logger;

        public BlobStorage(IConfiguration configuration, ILogger<BlobStorage> logger)
        {
            _logger = logger;
            try
            {
                var configPath = configuration["OCI:ConfigFilePath"] ?? throw new ArgumentNullException("OCI:ConfigFilePath is missing");
                var profile = configuration["OCI:Profile"] ?? "DEFAULT";
                _logger.LogInformation("Loading OCI config from {Path}", configPath);
                var config = ConfigFileReader.Parse(configPath, profile);
                var provider = new ConfigFileAuthenticationDetailsProvider(config);
                _client = new ObjectStorageClient(provider);
                _bucketName = configuration["OCI:BucketName"] ?? throw new ArgumentNullException("OCI:BucketName is missing");
                _namespace = configuration["OCI:Namespace"] ?? throw new ArgumentNullException("OCI:Namespace is missing");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize OCI client");
                throw;
            }
        }

        public async Task Upload(IFormFile file, string fileName)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("No file provided");
                if (!file.ContentType.StartsWith("image/"))
                    throw new ArgumentException("File must be an image");
                if (file.Length > 5 * 1024 * 1024) // Límite de 5MB
                    throw new ArgumentException("File size exceeds 5MB");

                using var stream = file.OpenReadStream();
                var putObjectRequest = new PutObjectRequest
                {
                    NamespaceName = _namespace,
                    BucketName = _bucketName,
                    ObjectName = fileName,
                    PutObjectBody = stream,
                    ContentLength = file.Length
                };
                _logger.LogInformation("Uploading file {FileName} to bucket {Bucket}", fileName, _bucketName);
                await _client.PutObject(putObjectRequest);
                _logger.LogInformation("File {FileName} uploaded successfully", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload file {FileName}", fileName);
                throw;
            }
        }
    }
}