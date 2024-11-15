namespace MediQueue.Domain.DTOs.Service
{
    public record ServiceForUpdateDto(
        int id, 
        string Name, 
        decimal Amount,
        int CategoryId,
        List<int>? AccountIds);
}
