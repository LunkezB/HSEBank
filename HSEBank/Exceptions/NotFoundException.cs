namespace HSEBank.Exceptions
{
    public class NotFoundException(string message) : DomainException(message);
}