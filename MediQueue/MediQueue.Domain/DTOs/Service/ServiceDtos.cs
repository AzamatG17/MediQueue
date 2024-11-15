using MediQueue.Domain.DTOs.Account;

namespace MediQueue.Domain.DTOs.Service
{
    public record ServiceDtos(
        int id,
        string Name, 
        decimal Amount,
        int CategoryId,
        string CategoryName,
        List<AccountHelperDto> Accounts
        );
}