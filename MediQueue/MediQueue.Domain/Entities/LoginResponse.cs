namespace MediQueue.Domain.Entities
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public Account User { get; set; }
    }
}
