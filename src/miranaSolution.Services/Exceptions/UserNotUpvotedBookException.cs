namespace miranaSolution.Services.Exceptions;

public sealed class UserNotUpvotedBookException : Exception
{
    public UserNotUpvotedBookException(string msg) : base(msg)
    {
        
    }
}