using MediQueue.Domain.DTOs.ServiceUsage;
using MediQueue.Domain.ResourceParameters;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IServiceUsageService
    {
        Task<IEnumerable<ServiceUsageDto>> GetAllServiceUsagesAsync(ServiceUsageResourceParametrs serviceUsageResourceParametrs);
        Task<ServiceUsageDto> GetServiceUsageByIdAsync(int id);
        Task<ServiceUsageDto> CreateServiceUsageAsync(ServiceUsageForCreateDto serviceUsageForCreate);
        Task<ServiceUsageDto> UpdateServiceUsageAsync(ServiceUsageForUpdateDto serviceUsageForUpdate);
        Task DeleteServiceUsageAsync(int id);
    }
}
