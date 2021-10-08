using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MegaUpload.Controllers
{
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;
        private readonly IHubContext<NotifyHub> _notifyHub;
        public UploadController(ILogger<UploadController> logger, IHubContext<NotifyHub> notifyHub)
        {
            _logger = logger;
            _notifyHub = notifyHub;
        }
        public async Task<IActionResult> Index(IFormFile file, [FromServices] IWebHostEnvironment env)
        {
            byte[] buffer = new byte[16 * 1024];
            long totalBytes = file.Length;
            using FileStream output = System.IO.File.Create(env.ContentRootPath + "/files/" + file.FileName);
            using Stream input = file.OpenReadStream();
            long totalReadBytes = 0;
            int readBytes;

            while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                await output.WriteAsync(buffer, 0, readBytes);
                totalReadBytes += readBytes;
                int progress = (int)((float)totalReadBytes / (float)totalBytes * 100.0);
                await _notifyHub.Clients.All.SendAsync("receiveProgress", progress);
                await Task.Delay(100); // It is only to make the process slower
            }

            TempData["message"] = "File Uploaded successfully!";

            return Ok();
        }
    }
}
