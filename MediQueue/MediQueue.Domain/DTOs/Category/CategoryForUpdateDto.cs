namespace MediQueue.Domain.DTOs.Category
{
    public record CategoryForUpdateDto(int Id, string CategoryName, List<int> GroupIds);
}
