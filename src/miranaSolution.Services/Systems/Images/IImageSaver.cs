namespace miranaSolution.Services.Systems.Images;

public interface IImageSaver
{
    Task<string> SaveImageAsync(Stream imageStream, string imageExtension);
}