using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.Group
{
    public record GroupForUpdateDto(int Id, string GroupName, List<int> CategoryIds);
}
