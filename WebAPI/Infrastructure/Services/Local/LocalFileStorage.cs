using WebAPI.Application.Interfaces.IExternalServices;

namespace WebAPI.Infrastructure.Services.Local;

public class LocalFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _contextAccessor;

    public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor contextAccessor)
    {
        _env = env;
        _contextAccessor = contextAccessor;
    }
    
    public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
    {
        var fileName = $"{Guid.NewGuid()}{extension}";

        string folder = Path.Combine(_env.WebRootPath, container);

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        string path = Path.Combine(folder, fileName);
        await File.WriteAllBytesAsync(path, content);

        var currentUrl = $"{_contextAccessor.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext.Request.Host}";

        var saveUrl = Path.Combine(currentUrl, container, fileName).Replace("\\", "/");

        return saveUrl;
    }

    public async Task<string> EditFile(byte[] content, string extension, string container, string path, string contentType)
    {
        await DeleteFile(path, container);

        return await SaveFile(content, extension, container, contentType);
    }

    public Task DeleteFile(string path, string container)
    {
        if (path is not null)
        {
            var fileName = Path.GetFileName(path);

            var fileDirectory = Path.Combine(_env.WebRootPath, container, fileName);

            if (File.Exists(fileDirectory))
                File.Delete(fileDirectory);
        }

        return Task.FromResult(0);
    }
}