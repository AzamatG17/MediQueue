using MediQueue.Domain.DTOs.Benefit;

namespace MediQueue.Domain.Interfaces.Services
{
    public interface IBenefitService
    {
        Task<IEnumerable<BenefitDto>> GetAllBenefitsAsync();
        Task<BenefitDto> GetBenefitByIdAsync(int id);
        Task<BenefitDto> CreateBenefitAsync(BenefitForCreateDto benefitForCreateDto);
        Task<BenefitDto> UpdateBenefitAsync(BenefitForUpdateDto benefitForUpdateDto);
        Task DeleteBenefitAsync(int id);
    }
}
