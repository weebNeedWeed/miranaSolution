namespace miranaSolution.Services.Exceptions;

public sealed class BookRatingAlreadyExistsException: Exception
{
    public BookRatingAlreadyExistsException(string msg) : base(msg)
    {
        
    }
}