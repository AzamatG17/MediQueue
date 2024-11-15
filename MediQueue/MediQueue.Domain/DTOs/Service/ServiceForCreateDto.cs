namespace MediQueue.Domain.DTOs.Service
{
    public record ServiceForCreateDto(
        string Name,
        decimal Amount,
        int CategoryId, 
        List<int>? AccountIds);
}
