namespace miranaSolution.Services.Exceptions;

public class BookNotFoundException : Exception
{
    public BookNotFoundException(string msg) : base(msg){}
}