namespace miranaSolution.Services.Exceptions;

public class FileNotFoundException : Exception
{
    public FileNotFoundException(string msg) : base(msg)
    {
    }
}