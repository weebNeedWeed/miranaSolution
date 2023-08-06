namespace miranaSolution.Services.Exceptions;

public class BookAlreadyExistsException : Exception
{
    public BookAlreadyExistsException(string msg) : base(msg)
    {
    }
}