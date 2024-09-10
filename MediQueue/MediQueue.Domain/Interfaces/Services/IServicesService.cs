using MediQueue.Domain.DTOs.Service;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IServicesService
    {
        Task<IEnumerable<ServiceDtos>> GetAllServicesAsync();
        Task<ServiceDtos> GetServiceByIdAsync(int id);
        Task<ServiceDtos> CreateServiceAsync(ServiceForCreateDto serviceForCreateDto);
        Task<ServiceDtos> UpdateServiceAsync(ServiceForUpdateDto serviceForUpdateDto);
        Task DeleteServiceAsync(int id);
    }
}
