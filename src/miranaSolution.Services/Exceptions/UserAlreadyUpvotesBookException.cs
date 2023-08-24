namespace miranaSolution.Services.Exceptions;

public sealed class UserAlreadyUpvotesBookException : Exception
{
    public UserAlreadyUpvotesBookException(string msg) : base(msg) {}
}