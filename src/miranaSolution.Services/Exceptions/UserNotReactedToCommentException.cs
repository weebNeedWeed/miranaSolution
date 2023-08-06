namespace miranaSolution.Services.Exceptions;

public class UserNotReactedToCommentException : Exception
{
    public UserNotReactedToCommentException(string msg) : base(msg)
    {
    }
}