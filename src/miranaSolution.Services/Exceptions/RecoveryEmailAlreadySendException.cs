namespace miranaSolution.Services.Exceptions;

public class RecoveryEmailAlreadySendException : Exception
{
    public RecoveryEmailAlreadySendException(string msg) : base(msg)
    {
    }
}