namespace miranaSolution.Business.Systems.Files;

public interface IFileService
{
    Task<string> SaveFileAsync(Stream stream, string fileName);
    Task DeleteFileAsync(string fileName);
    string GetUrl(string fileName);
}