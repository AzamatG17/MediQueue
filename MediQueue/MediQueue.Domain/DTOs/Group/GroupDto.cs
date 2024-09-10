using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.Group
{
    public record GroupDto(int Id, string GroupName, List<GroupInfoResponse> CategoryIds);
}
