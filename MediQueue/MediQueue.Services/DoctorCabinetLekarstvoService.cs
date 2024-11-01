using MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class DoctorCabinetLekarstvoService : IDoctorCabinetLekarstvoService
{
    private readonly IDoctorCabinetLekarstvoRepository _repository;
    private readonly IDoctorCabinetRepository _cabinetRepository;
    private readonly IPartiyaRepository _partiyaRepository;

    public DoctorCabinetLekarstvoService(IDoctorCabinetLekarstvoRepository repository, IDoctorCabinetRepository cabinetRepository, IPartiyaRepository partiyaRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _cabinetRepository = cabinetRepository ?? throw new ArgumentNullException(nameof(cabinetRepository));
        _partiyaRepository = partiyaRepository ?? throw new ArgumentNullException(nameof(partiyaRepository));
    }

    public async Task<IEnumerable<DoctorCabinetLekarstvoDto>> GetAllDoctorCabinetLekarstvosAsync()
    {
        var cabinetLekarstvos = await _repository.FindAllDoctorCabinetLekarstvoAsync();

        if (cabinetLekarstvos == null) return null;

        return cabinetLekarstvos.Select(MapToDoctorCabinetLekarstvoDto).ToList();
    }

    public async Task<DoctorCabinetLekarstvoDto> GetDoctorCabinetLekarstvoByIdAsync(int id)
    {
        var cabinetLekarstvo = await _repository.FindByIdDoctorCabinetLekarstvoAsync(id);

        if (cabinetLekarstvo == null) return null;

        return MapToDoctorCabinetLekarstvoDto(cabinetLekarstvo);
    }

    public async Task<DoctorCabinetLekarstvoDto> CreateDoctorCabinetLekarstvoAsync(DoctorCabinetLekarstvoForCreateDto doctorCabinetLekarstvoForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(doctorCabinetLekarstvoForCreateDto);

        if (!await _cabinetRepository.IsExistByIdAsync(doctorCabinetLekarstvoForCreateDto.DoctorCabinetId))
        {
            throw new ArgumentException($"Doctor Cabinet with id: {doctorCabinetLekarstvoForCreateDto.DoctorCabinetId} does not exist");
        }

        var partiya = await _partiyaRepository.FindByIdAsync(doctorCabinetLekarstvoForCreateDto.PartiyaId);
        if (partiya == null)
        {
            throw new ArgumentException($"Partiya with id: {doctorCabinetLekarstvoForCreateDto.PartiyaId} does not exist");
        }

        if (partiya.TotalQuantity.HasValue && partiya.TotalQuantity < doctorCabinetLekarstvoForCreateDto.Quantity)
        {
            throw new InvalidOperationException("Not enough quantity in Partiya for this operation.");
        }

        // Deduct the quantity from Partiya
        partiya.TotalQuantity -= doctorCabinetLekarstvoForCreateDto.Quantity;

        // Update Partiya in the database
        await _partiyaRepository.UpdateAsync(partiya);

        var cabinetLekarstvo = new DoctorCabinetLekarstvo
        {
            Quantity = (decimal)doctorCabinetLekarstvoForCreateDto.Quantity,
            DoctorCabinetId = doctorCabinetLekarstvoForCreateDto.DoctorCabinetId,
            PartiyaId = doctorCabinetLekarstvoForCreateDto.PartiyaId
        };

        await _repository.CreateAsync(cabinetLekarstvo);

        return MapToDoctorCabinetLekarstvoDto(cabinetLekarstvo);
    }

    public Task<DoctorCabinetLekarstvoDto> UpdateDoctorCabinetLekarstvoAsync(DoctorCabinetLekarstvoForUpdateDto doctorCabinetLekarstvoForUpdateDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteDoctorCabinetLekarstvoAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task UseLekarstvoAsync(int id, decimal amount)
    {
        var lekarstvo = await _repository.FindByIdAsync(id);
        if (lekarstvo == null)
        {
            throw new KeyNotFoundException($"Lekarstvo with id {id} not found.");
        }

        if (amount <= 0)
            throw new ArgumentException("Quantity must be included.");

        if (amount > lekarstvo.Quantity - lekarstvo.Partiya.PriceQuantity)
            throw new InvalidOperationException("Not enough medicine to use.");

        lekarstvo.Quantity -= amount;

        await _repository.UpdateAsync(lekarstvo);
    }

    public async Task AddLekarstvoQuantityAsync(int id, decimal amount)
    {
        var lekarstvo = await _repository.FindByIdAsync(id);
        if (lekarstvo == null)
        {
            throw new KeyNotFoundException($"Lekarstvo with id {id} not found.");
        }

        if (amount <= 0)
            throw new ArgumentException("Quantity must be included.");

        lekarstvo.Quantity += amount;

        await _repository.UpdateAsync(lekarstvo);
    }

    private DoctorCabinetLekarstvoDto MapToDoctorCabinetLekarstvoDto(DoctorCabinetLekarstvo d)
    {
        return new DoctorCabinetLekarstvoDto(
            d.Id,
            d.Quantity,
            d.DoctorCabinetId,
            $"{d.DoctorCabinet?.Account?.LastName} {d.DoctorCabinet?.Account?.FirstName} {d.DoctorCabinet?.Account?.SurName}" ?? "",
            d.PartiyaId,
            d.Partiya?.Lekarstvo?.Name ?? ""
            );
    }
}
