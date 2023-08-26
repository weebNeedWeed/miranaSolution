namespace miranaSolution.Services.Exceptions;

public sealed class ChapterAlreadyExistsException : Exception
{
    public ChapterAlreadyExistsException(string msg) : base(msg) {}
}