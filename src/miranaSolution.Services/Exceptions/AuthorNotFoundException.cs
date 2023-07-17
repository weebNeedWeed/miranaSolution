namespace miranaSolution.Services.Exceptions;

public class AuthorNotFoundException : Exception
{
    public AuthorNotFoundException(string msg) : base(msg) {}
}