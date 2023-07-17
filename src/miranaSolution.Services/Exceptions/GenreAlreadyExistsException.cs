namespace miranaSolution.Services.Exceptions;

public class GenreAlreadyExistsException : Exception
{
    public GenreAlreadyExistsException(string msg) : base(msg) {}
}