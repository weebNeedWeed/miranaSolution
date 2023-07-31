namespace miranaSolution.Services.Exceptions;

public class BookRatingNotFoundException : Exception
{
    public BookRatingNotFoundException(string msg) : base(msg) {}
}