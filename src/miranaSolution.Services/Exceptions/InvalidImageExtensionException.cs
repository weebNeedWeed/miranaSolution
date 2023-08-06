namespace miranaSolution.Services.Exceptions;

public class InvalidImageExtensionException : Exception
{
    public InvalidImageExtensionException(string msg) : base(msg)
    {
    }
}