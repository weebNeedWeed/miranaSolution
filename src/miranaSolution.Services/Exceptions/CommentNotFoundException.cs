namespace miranaSolution.Services.Exceptions;

public class CommentNotFoundException : Exception
{
    public CommentNotFoundException(string msg) : base(msg) {}
}