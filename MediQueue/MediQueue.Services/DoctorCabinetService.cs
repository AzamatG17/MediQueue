using MediQueue.Domain.DTOs.DoctorCabinet;
using MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class DoctorCabinetService : IDoctorCabinetService
{
    private readonly IDoctorCabinetRepository _repository;
    private readonly IAccountRepository _accountRepository;

    public DoctorCabinetService(IDoctorCabinetRepository repository, IAccountRepository accountRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
    }
    
    public async Task<IEnumerable<DoctorCabinetDto>> GetAllDoctorCabinetsAsync()
    {
        var doctorCabinets = await _repository.FindAllDoctorCabinetsAsync();

        if (doctorCabinets == null) return null;

        return doctorCabinets.Select(MapToDoctorCabinetDto).ToList();
    }

    public async Task<DoctorCabinetDto> GetDoctorCabinetByIdAsync(int id)
    {
        var doctorCabinet = await _repository.FindByIdDoctorCabinetAsync(id)
            ?? throw new KeyNotFoundException($"Doctor Cabinet with id: {id} does not exist.");

        return MapToDoctorCabinetDto(doctorCabinet);
    }

    public async Task<DoctorCabinetDto> CreateDoctorCabinetAsync(DoctorCabinetForCreate doctorCabinetForCreate)
    {
        ArgumentNullException.ThrowIfNull(doctorCabinetForCreate);

        if (!await _accountRepository.IsExistByIdAsync(doctorCabinetForCreate.AccountId))
        {
            throw new ArgumentException($"Account with id: {doctorCabinetForCreate.AccountId} does not exist");
        }

        var doctorCabinet = new DoctorCabinet
        {
            RoomNumber = doctorCabinetForCreate.RoomNumber,
            AccountId = doctorCabinetForCreate.AccountId,
        };

        await _repository.CreateAsync(doctorCabinet);

        return MapToDoctorCabinetDto(doctorCabinet);
    }

    public async Task<DoctorCabinetDto> UpdateDoctorCabinetAsync(DoctorCabinetForUpdate doctorCabinetForUpdate)
    {
        ArgumentNullException.ThrowIfNull(doctorCabinetForUpdate);

        if (!await _repository.IsExistByIdAsync(doctorCabinetForUpdate.Id))
        {
            throw new KeyNotFoundException($"DoctorCabinet with id: {doctorCabinetForUpdate.Id} does not exist");
        }

        if (!await _accountRepository.IsExistByIdAsync(doctorCabinetForUpdate.AccountId))
        {
            throw new ArgumentException($"Account with id: {doctorCabinetForUpdate.AccountId} does not exist");
        }

        var doctorCabinet = await _repository.FindByIdAsync(doctorCabinetForUpdate.Id)
            ?? throw new KeyNotFoundException($"Doctor Cabinet with id: {doctorCabinetForUpdate.Id} does not exist.");

        doctorCabinet.RoomNumber = doctorCabinetForUpdate?.RoomNumber;
        doctorCabinet.AccountId = doctorCabinetForUpdate?.AccountId;

        await _repository.UpdateAsync(doctorCabinet);

        return MapToDoctorCabinetDto(doctorCabinet);
    }

    public async Task DeleteDoctorCabinetAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    private static DoctorCabinetDto MapToDoctorCabinetDto(DoctorCabinet d)
    {
        return new DoctorCabinetDto(
            d.Id,
            d.RoomNumber,
            d.AccountId,
            $"{d.Account?.LastName ?? ""} {d.Account?.FirstName ?? ""} {d.Account?.SurName ?? ""}".Trim(),
            d.DoctorCabinetLekarstvos != null
                ? d.DoctorCabinetLekarstvos.Select(MapToDoctorCabinetLekarstvoDto).ToList()
                : new List<DoctorCabinetLekarstvoDto>()
            );
    }

    private static DoctorCabinetLekarstvoDto MapToDoctorCabinetLekarstvoDto(DoctorCabinetLekarstvo d)
    {
        return new DoctorCabinetLekarstvoDto(
            d.Id,
            d.Quantity,
            d.CreateDate ?? DateTime.MinValue,
            d.DoctorCabinetId,
            $"{d.DoctorCabinet?.Account?.LastName ?? ""} {d.DoctorCabinet?.Account?.FirstName ?? ""} {d.DoctorCabinet?.Account?.SurName ?? ""}".Trim(),
            d.PartiyaId,
            d.Partiya?.Lekarstvo?.Name ?? ""
            );
    }
}
