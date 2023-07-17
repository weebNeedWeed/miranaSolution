using Microsoft.AspNetCore.Hosting;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Systems.Files;

public class FileService : IFileService
{
    private readonly string _root;
    private readonly string _folder = "uploads";

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _root = Path.Combine(webHostEnvironment.WebRootPath, _folder);
        if (!File.Exists(_root)) Directory.CreateDirectory(_root);
    }
    
    public async Task<bool> SaveFileAsync(Stream fileStream, string fileName)
    {
        var filePath = Path.Combine(_root, fileName);
        if (File.Exists(filePath))
        {
            throw new FileAlreadyExistsException("The file with given name already exists.");
        }
        
        await using var file = File.Create(filePath);
        await fileStream.CopyToAsync(file);

        return true;
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        var filePath = Path.Combine(_root, fileName);
        if (!File.Exists(filePath))
        {
            throw new Exceptions.FileNotFoundException("The file with given name does not exist.");
        }

        await Task.Run(() => File.Delete(filePath));

        return true;
    }

    public string GetRelativeFilePath(string fileName)
    {
        var path = _folder + "/" + fileName;
        return path;
    }
}