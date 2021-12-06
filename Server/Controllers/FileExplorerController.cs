using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileExplorerController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                var listing = GetDirectoryListing(new DirectoryInfo(Directory.GetCurrentDirectory()));
                return Ok(listing);
            }

            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                return PhysicalFile(fileInfo.FullName, "application/octet-stream", fileInfo.Name);
            }

            var dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
            {
                var listing = GetDirectoryListing(dirInfo);

                return Ok(listing);
            }

            return BadRequest();
        }

        [HttpGet("testTimeout")]
        public ActionResult TestTimeout(int? time)
        {
            return new SlowResult(time ?? 100);
        }

        private static DirectoryListingDto GetDirectoryListing(DirectoryInfo dirInfo)
        {
            var files = new Lazy<List<FileDto>>();
            var dirs = new Lazy<List<string>>();
            foreach (var item in dirInfo.EnumerateFileSystemInfos())
            {
                switch (item)
                {
                    case DirectoryInfo dir:
                        dirs.Value.Add(dir.Name);
                        break;

                    case FileInfo f:
                        files.Value.Add(new FileDto { Name = f.Name, Size = f.Length });
                        break;

                    default:
                        break;
                }
            }

            return new DirectoryListingDto
            {
                FullName = dirInfo.FullName,
                Files = files.IsValueCreated ? files.Value : null,
                Directories = dirs.IsValueCreated ? dirs.Value : null,
            };
        }
    }

    public class SlowResult : ActionResult
    {
        private readonly int totalActionTimeInSeconds;

        public SlowResult(int totalActionTimeInSeconds)
        {
            this.totalActionTimeInSeconds = totalActionTimeInSeconds;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 200;
            response.ContentType = "application/octet-stream";
            response.ContentLength = totalActionTimeInSeconds;

            await response.StartAsync();

            for (int i = 0; i < totalActionTimeInSeconds; i++)
            {
                response.Body.WriteByte(1);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            await response.CompleteAsync();
        }
    }
}