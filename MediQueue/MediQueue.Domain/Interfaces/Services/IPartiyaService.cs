using MediQueue.Domain.DTOs.Partiya;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IPartiyaService
    {
        Task<IEnumerable<PartiyaDto>> GetAllPartiyastvosAsync();
        Task<PartiyaDto> GetPartiyaByIdAsync(int id);
        Task<PartiyaDto> CreatePartiyaAsync(PartiyaForCreateDto partiya);
        Task<PartiyaDto> UpdatePartiyaAsync(PartiyaForUpdateDto partiya);
        Task DeletePartiyaAsync(int id);
    }
}
