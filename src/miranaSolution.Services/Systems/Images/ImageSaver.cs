using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Files;
using FileNotFoundException = miranaSolution.Services.Exceptions.FileNotFoundException;

namespace miranaSolution.Services.Systems.Images;

public class ImageSaver : IImageSaver
{
    private readonly IFileService _fileService;

    public ImageSaver(IFileService fileService)
    {
        _fileService = fileService;
    }

    /// <exception cref="InvalidImageExtensionException">
    ///     Thrown when the extension of thumbnail image is not allowed
    /// </exception>
    public async Task<string> SaveImageAsync(Stream imageStream, string imageExtension)
    {
        if (!IsValidExtension(imageExtension))
            throw new InvalidImageExtensionException(
                "Invalid thumbnail image's extension. Only allow .jpg, .png and .jpeg.");

        var newName = $"{Guid.NewGuid().ToString()}{imageExtension}";
        await _fileService.SaveFileAsync(imageStream, newName);

        return _fileService.GetRelativeFilePath(newName);
    }

    public async Task DeleteImageIfExistAsync(string imagePath)
    {
        // Extracting the file name from a path having the format "<dir>/filename.ext"
        var split = imagePath.Split("/");
        if (split.Length != 2) return;

        try
        {
            var name = split[1];
            await _fileService.DeleteFileAsync(name);
        }
        catch (FileNotFoundException)
        {
        }
    }

    private bool IsValidExtension(string fileExtension)
    {
        var allowedExt = new List<string> { ".jpg", ".jpeg", ".png" };
        return allowedExt.Contains(fileExtension);
    }
}