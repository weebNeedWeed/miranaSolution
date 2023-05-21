using Microsoft.AspNetCore.Hosting;

namespace miranaSolution.Business.Systems.Files;

public class FileService : IFileService
{
    private readonly string _root;
    private readonly string _folder = "users";

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _root = Path.Combine(webHostEnvironment.WebRootPath, _folder);
        if (!File.Exists(_root))
        {
            Directory.CreateDirectory(_root);
        }
    }

    public async Task<string> SaveFileAsync(Stream stream, string fileName)
    {
        var filePath = Path.Combine(_root, fileName);
        await using var file = File.Create(filePath);
        await stream.CopyToAsync(file);

        return fileName;
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var filePath = Path.Combine(_root, fileName);
        if (!File.Exists(filePath))
        {
            return;
        }

        await Task.Run(() => File.Delete(filePath));
    }

    public string GetUrl(string fileName)
    {
        return Path.Combine(_root, fileName);
    }
}