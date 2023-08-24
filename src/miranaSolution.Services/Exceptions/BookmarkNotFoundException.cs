namespace miranaSolution.Services.Exceptions;

public sealed class BookmarkNotFoundException: Exception
{
    public BookmarkNotFoundException(string msg) : base(msg) {}
}