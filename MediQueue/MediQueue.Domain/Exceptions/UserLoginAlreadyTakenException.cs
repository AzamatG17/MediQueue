namespace MediQueue.Domain.Exceptions;

public sealed class UserLoginAlreadyTakenException : ApplicationException
{
    public UserLoginAlreadyTakenException(string message) : base(message) { }
}
