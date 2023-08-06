namespace miranaSolution.Services.Exceptions;

public class UserAlreadyReactsToCommentException : Exception
{
    public UserAlreadyReactsToCommentException(string msg) : base(msg)
    {
    }
}