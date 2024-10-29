using MediQueue.Domain.DTOs.Partiya;
using MediQueue.Domain.DTOs.ScladLekarstvo;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IPartiyaService
    {
        Task<IEnumerable<PartiyaDto>> GetAllPartiyastvosAsync();
        Task<PartiyaDto> GetPartiyaByIdAsync(int id);
        Task<PartiyaDto> CreatePartiyaAsync(ScladLekarstvoForCreate scladLekarstvoForCreate);
        Task<PartiyaDto> UpdatePartiyaAsync(ScladLekarstvoForUpdate scladLekarstvoForUpdate);
        Task DeletePartiyaAsync(int id);
    }
}
