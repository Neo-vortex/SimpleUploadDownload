using Microsoft.AspNetCore.Mvc;

namespace SimpleUploadDownload.Controllers;

[ApiController]
[Route("api")]
public class FileUploader : ControllerBase
{
    
    [HttpPost]
    [Route("upload")]
    public Task<IActionResult> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return Task.FromResult<IActionResult>(BadRequest("file not selected"));
        }

        var fileName = Random.Shared.Next() + file.FileName;
        var path = Path.Combine(Directory.GetCurrentDirectory(),fileName );

        using (var stream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Task.FromResult<IActionResult>(Ok(fileName));
    }
    
    [HttpGet]
    [Route("download")]
    public Task<IActionResult> DownloadFile(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
        var memory = new MemoryStream();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            stream.CopyTo(memory);
        }
        memory.Position = 0;
        return Task.FromResult<IActionResult>(File(memory, "application/octet-stream", Path.GetFileName(path)));
    }
}