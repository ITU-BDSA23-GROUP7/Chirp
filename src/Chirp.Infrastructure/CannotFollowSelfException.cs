namespace Chirp.Infrastructure;
public class CannotFollowSelfException : Exception
{
    public CannotFollowSelfException() { }
    public CannotFollowSelfException(string message) : base(message) { }
}