using MediQueue.Domain.DTOs.Category;

namespace MediQueue.Domain.DTOs.Group
{
    public record GroupForAllDateDto(int Id, string GroupName, List<CategoryForGroupDto> CategoryForGroupDtos);
}
