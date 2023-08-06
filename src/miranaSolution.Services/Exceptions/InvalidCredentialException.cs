namespace miranaSolution.Services.Exceptions;

public class InvalidCredentialException : Exception
{
    public InvalidCredentialException(string msg) : base(msg)
    {
    }
}