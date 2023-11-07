public class UsernameNotFoundException : Exception
{
    public UsernameNotFoundException() { }
    public UsernameNotFoundException(string message) : base (message) { }
}