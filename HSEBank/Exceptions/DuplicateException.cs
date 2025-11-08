namespace HSEBank.Exceptions
{
    public class DuplicateException(string message) : DomainException(message);
}