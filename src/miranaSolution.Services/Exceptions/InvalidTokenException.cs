namespace miranaSolution.Services.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(string msg) : base(msg)
    {
    }
}