using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogUp.Controllers;

[ApiController]
[Route("upload")]
public class UploadController(IAmazonS3 s3, ILogger<UploadController> logger) : ControllerBase
{
    [HttpPost(Name = "Upload")]
    public async Task<IActionResult> Post(IFormFile _)
    {
        const string bucketName = "logstorage";
        
        logger.Log(LogLevel.Information, "Upload request received");
        
        try
        {
            using var logFileStreamReader = new StreamReader(_.OpenReadStream());

            var uuid = Guid.NewGuid().ToString();
            var uploadDate = DateOnly.FromDateTime(DateTime.Now);
            var expiryDate = uploadDate.AddMonths(12);

            await using var stream = _.OpenReadStream();

            if (!await s3.DoesS3BucketExistAsync(bucketName))
            { 
                await s3.PutBucketAsync(bucketName);
            };
            
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = uuid,
                InputStream = stream
            };
            
            request.Metadata.Add("Content-Type", _.ContentType);
            request.Metadata.Add("Expiry-Date", expiryDate.ToString("yyyy-MM-dd"));
            
            await s3.PutObjectAsync(request);
            
            return Ok($"File {_.FileName} uploaded to storage successfully with uuid {uuid}");
        }
        catch (Exception ex)
        {
            logger.LogError($"Caught error while processing Post request: {ex.Message}");
            
            return BadRequest(ex.Message);
        }
    }
}