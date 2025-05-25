using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LogUp.Controllers;

[ApiController]
[Route("{uuid}")]
public class GetController(IAmazonS3 s3, ILogger<GetController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(string uuid)
    {
        const string bucketName = "logstorage";
        
        if (!await s3.DoesS3BucketExistAsync(bucketName)) return NotFound($"Bucket {bucketName} does not exist.");

        var getFile = new GetObjectRequest()
        {
            BucketName = bucketName,
            Key = uuid
        };

        try
        {
            using var response = await s3.GetObjectAsync(getFile);
            
            using var reader = new StreamReader(response.ResponseStream);
            var content = await reader.ReadToEndAsync();

            return Content(content, "text/plain");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return Content($"No log found with uuid {uuid}", "text/plain");
        }
    }
}