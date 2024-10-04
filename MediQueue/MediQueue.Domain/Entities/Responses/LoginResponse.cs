namespace MediQueue.Domain.Entities.Responses;

public class LoginResponse
{
    public string Token { get; set; }
    public Account User { get; set; }
}
