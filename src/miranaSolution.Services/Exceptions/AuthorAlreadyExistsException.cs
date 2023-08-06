namespace miranaSolution.Services.Exceptions;

public class AuthorAlreadyExistsException : Exception
{
    public AuthorAlreadyExistsException(string msg) : base(msg)
    {
    }
}