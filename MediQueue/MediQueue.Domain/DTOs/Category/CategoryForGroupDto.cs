using MediQueue.Domain.DTOs.Service;

namespace MediQueue.Domain.DTOs.Category
{
    public record CategoryForGroupDto(int Id, string CategoryName, ICollection<ServiceHelperDto> ServiceDtos);
}
