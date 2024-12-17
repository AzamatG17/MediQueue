namespace MediQueue.Domain.Exceptions;

public class UserLoginAlreadyTakenException : ApplicationException
{
    public UserLoginAlreadyTakenException(string message) : base(message) { }
}
