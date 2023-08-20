namespace miranaSolution.Services.Exceptions;

public sealed class SlideNotFoundException : Exception
{
    public SlideNotFoundException(string msg) : base(msg) {}
}