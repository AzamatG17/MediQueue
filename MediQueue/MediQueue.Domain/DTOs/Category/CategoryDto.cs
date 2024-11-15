using MediQueue.Domain.DTOs.Service;
using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.Category
{
    public record CategoryDto(int Id, string CategoryName, List<GroupInfoResponse> GroupIds, ICollection<ServiceHelperDto> ServiceDtos);
}
