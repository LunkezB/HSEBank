namespace HSEBank.Exceptions
{
    public class ValidationException(string message) : DomainException(message);
}