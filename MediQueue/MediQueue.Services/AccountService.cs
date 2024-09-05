using AutoMapper;
using MediQueue.Domain.DTOs.Account;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts =  await _accountRepository.FindAllAsync();

            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<AccountDto> GetAccountByIdAsync(int id)
        {
            var account = await _accountRepository.FindByIdAsync(id);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with id {id} not found.");
            }

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> CreateAccountAsync(AccountForCreateDto accountForCreateDto)
        {
            if (accountForCreateDto == null)
            {
                throw new ArgumentNullException(nameof(accountForCreateDto));
            }

            var accountEntity = _mapper.Map<Account>(accountForCreateDto);

            await _accountRepository.CreateAsync(accountEntity);

            return _mapper.Map<AccountDto>(accountEntity);
        }

        public async Task<AccountDto> UpdateAccountAsync(AccountForUpdateDto accountForUpdateDto)
        {
            if (accountForUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(accountForUpdateDto));
            }

            var account = _mapper.Map<Account>(accountForUpdateDto);

            await _accountRepository.UpdateAsync(account);

            var accountDto = _mapper.Map<AccountDto>(account);

            return _mapper.Map<AccountDto>(account);      
        }

        public async Task DeleteAccountAsync(int id)
        {
            await _accountRepository.DeleteAsync(id);
        }
    }
}
