namespace miranaSolution.Services.Systems.Files;

public interface IFileService
{
    Task<bool> SaveFileAsync(Stream fileStream, string fileName);
    
    Task<bool> DeleteFileAsync(string fileName);
    
    string GetFilePath(string fileName);
}