namespace miranaSolution.Utilities.Exceptions;

public class MiranaBusinessException : Exception
{
    public MiranaBusinessException()
    {
    }

    public MiranaBusinessException(string msg) : base(msg)
    {
    }

    public MiranaBusinessException(string msg, Exception inner) : base(msg, inner)
    {
    }
}