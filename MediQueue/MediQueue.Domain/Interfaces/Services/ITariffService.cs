using MediQueue.Domain.DTOs.Tariff;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface ITariffService
    {
        Task<IEnumerable<TariffDto>> GetAllTariffsAsync();
        Task<TariffDto> GetTariffByIdAsync(int id);
        Task<TariffDto> CreateTariffAsync(TariffForCreateDto tariffForCreateDto);
        Task<TariffDto> UpdateTariffAsync(TariffForUpdateDto tariffForUpdateDto);
        Task DeleteTariffAsync(int id);
    }
}
