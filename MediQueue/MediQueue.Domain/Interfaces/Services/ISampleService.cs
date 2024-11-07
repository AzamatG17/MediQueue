using MediQueue.Domain.DTOs.Sample;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface ISampleService
    {
        Task<IEnumerable<SampleDto>> GetAllSamplesAsync();
        Task<SampleDto> GetSampleByIdAsync(int id);
        Task<SampleDto> CreateSampleAsync(SampleForCreateDto sampleForCreateDto);
        Task<SampleDto> UpdateSampleAsync(SampleForUpdateDto sampleForUpdate);
        Task DeleteSampleAsync(int id);
    }
}
