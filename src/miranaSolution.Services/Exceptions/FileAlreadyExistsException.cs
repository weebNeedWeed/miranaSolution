namespace miranaSolution.Services.Exceptions;

public class FileAlreadyExistsException : Exception
{
    public FileAlreadyExistsException(string msg) : base(msg)
    {
    }
}