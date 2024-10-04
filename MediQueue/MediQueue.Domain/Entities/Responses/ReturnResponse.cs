namespace MediQueue.Domain.Entities.Responses;

public class ReturnResponse
{
    public int Code { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
}
