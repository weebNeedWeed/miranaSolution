namespace miranaSolution.Services.Exceptions;

public class GenreNotFoundException : Exception
{
    public GenreNotFoundException(string msg) : base(msg)
    {
    }
}