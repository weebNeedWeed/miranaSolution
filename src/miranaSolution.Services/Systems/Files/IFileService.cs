namespace miranaSolution.Services.Systems.Files;

public interface IFileService
{
    Task<bool> SaveFileAsync(Stream fileStream, string fileName);

    Task<bool> DeleteFileAsync(string fileName);

    string GetRelativeFilePath(string fileName);
}