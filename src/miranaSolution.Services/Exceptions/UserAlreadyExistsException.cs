namespace miranaSolution.Services.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string msg) : base(msg) {}
}