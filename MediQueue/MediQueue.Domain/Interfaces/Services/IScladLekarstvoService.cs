using MediQueue.Domain.DTOs.ScladLekarstvo;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IScladLekarstvoService
    {
        Task<IEnumerable<ScladLekarstvoDto>> GetAllScladLekarstvosAsync();
        Task<ScladLekarstvoDto> GetScladLekarstvoByIdAsync(int id);
        Task<ScladLekarstvoDto> CreateScladLekarstvoAsync(ScladLekarstvoForCreate scladLekarstvoForCreate);
        Task<ScladLekarstvoDto> UpdateScladLekarstvoAsync(ScladLekarstvoForUpdate scladLekarstvoForUpdate);
        Task DeleteScladLekarstvoAsync(int id);
    }
}
