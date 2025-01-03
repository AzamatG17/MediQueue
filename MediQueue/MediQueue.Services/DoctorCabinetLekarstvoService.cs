using MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Exceptions;
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
        var cabinetLekarstvo = await _repository.FindByIdDoctorCabinetLekarstvoAsync(id)
            ?? throw new EntityNotFoundException($"Doctor Cabinet Lekarstvo with id: {id} does not exist.");

        return MapToDoctorCabinetLekarstvoDto(cabinetLekarstvo);
    }

    public async Task<DoctorCabinetLekarstvoDto> CreateDoctorCabinetLekarstvoAsync(DoctorCabinetLekarstvoForCreateDto doctorCabinetLekarstvoForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(doctorCabinetLekarstvoForCreateDto);

        if (!await _cabinetRepository.IsExistByIdAsync(doctorCabinetLekarstvoForCreateDto.DoctorCabinetId))
        {
            throw new ArgumentException($"Doctor Cabinet with id: {doctorCabinetLekarstvoForCreateDto.DoctorCabinetId} does not exist");
        }

        if (doctorCabinetLekarstvoForCreateDto.DoctorCabinetResponses == null ||
        !doctorCabinetLekarstvoForCreateDto.DoctorCabinetResponses.Any())
        {
            throw new ArgumentException("DoctorCabinetResponses cannot be null or empty");
        }

        var cabinetLekarstvoList = new List<DoctorCabinetLekarstvo>();

        foreach (var response in doctorCabinetLekarstvoForCreateDto.DoctorCabinetResponses)
        {
            if (!response.PartiyaId.HasValue || !response.Quantity.HasValue)
            {
                throw new ArgumentException("Each response must have valid PartiyaId and Quantity");
            }

            var partiya = await _partiyaRepository.FindByIdAsync(response.PartiyaId.Value)
                ?? throw new ArgumentException($"Partiya with id: {response.PartiyaId} does not exist");

            if (partiya.TotalQuantity.HasValue && partiya.TotalQuantity < response.Quantity.Value)
            {
                throw new InvalidOperationException($"Not enough quantity in Partiya with id: {response.PartiyaId} for this operation.");
            }

            partiya.TotalQuantity -= response.Quantity.Value;
            await _partiyaRepository.UpdateAsync(partiya);

            var cabinetLekarstvo = new DoctorCabinetLekarstvo
            {
                Quantity = response.Quantity.Value,
                DoctorCabinetId = doctorCabinetLekarstvoForCreateDto.DoctorCabinetId,
                PartiyaId = response.PartiyaId.Value,
                CreateDate = DateTime.Now,
            };

            cabinetLekarstvoList.Add(cabinetLekarstvo);
        }

        foreach (var lekarstvo in cabinetLekarstvoList)
        {
            await _repository.CreateAsync(lekarstvo);
        }

        return MapToDoctorCabinetLekarstvoDto(cabinetLekarstvoList.Last());
    }

    public async Task<DoctorCabinetLekarstvoDto> UpdateDoctorCabinetLekarstvoAsync(DoctorCabinetLekarstvoForUpdateDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(updateDto);

        var existingLekarstvo = await _repository.FindByIdAsync(updateDto.Id)
            ?? throw new ArgumentException($"DoctorCabinetLekarstvo with id: {updateDto.Id} does not exist");

        if (updateDto.DoctorCabinetId.HasValue)
        {
            if (!await _cabinetRepository.IsExistByIdAsync(updateDto.DoctorCabinetId.Value))
            {
                throw new ArgumentException($"Doctor Cabinet with id: {updateDto.DoctorCabinetId.Value} does not exist");
            }
            existingLekarstvo.DoctorCabinetId = updateDto.DoctorCabinetId.Value;
        }

        if (updateDto.DoctorCabinetResponses != null && updateDto.DoctorCabinetResponses.Any())
        {
            foreach (var response in updateDto.DoctorCabinetResponses)
            {
                if (!response.PartiyaId.HasValue || !response.Quantity.HasValue)
                {
                    throw new ArgumentException("Each response must have valid PartiyaId and Quantity");
                }

                var partiya = await _partiyaRepository.FindByIdAsync(response.PartiyaId.Value)
                    ?? throw new ArgumentException($"Partiya with id: {response.PartiyaId} does not exist");

                if (partiya.TotalQuantity.HasValue && partiya.TotalQuantity < response.Quantity.Value)
                {
                    throw new InvalidOperationException($"Not enough quantity in Partiya with id: {response.PartiyaId} for this operation.");
                }

                partiya.TotalQuantity -= response.Quantity.Value;
                await _partiyaRepository.UpdateAsync(partiya);

                existingLekarstvo.PartiyaId = response.PartiyaId.Value;
                existingLekarstvo.Quantity = response.Quantity.Value;
            }
        }

        await _repository.UpdateAsync(existingLekarstvo);

        return MapToDoctorCabinetLekarstvoDto(existingLekarstvo);
    }

    public async Task DeleteDoctorCabinetLekarstvoAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task UseLekarstvoAsync(int id, decimal amount)
    {
        var lekarstvo = await _repository.FindByIdDoctorCabinetLekarstvoAsync(id)
            ?? throw new EntityNotFoundException($"Lekarstvo with id {id} not found.");

        if (amount <= 0)
            throw new ArgumentException("Quantity must be included.");

        if (amount > lekarstvo.Quantity)
            throw new InvalidOperationException("Not enough medicine to use.");

        lekarstvo.Quantity -= amount;

        await _repository.UpdateAsync(lekarstvo);
    }

    public async Task AddLekarstvoQuantityAsync(int id, decimal amount)
    {
        var lekarstvo = await _repository.FindByIdAsync(id)
            ?? throw new EntityNotFoundException($"Lekarstvo with id {id} not found.");
        
        if (amount <= 0)
            throw new ArgumentException("Quantity must be included.");

        lekarstvo.Quantity += amount;

        await _repository.UpdateAsync(lekarstvo);
    }

    private static DoctorCabinetLekarstvoDto MapToDoctorCabinetLekarstvoDto(DoctorCabinetLekarstvo d)
    {
        return new DoctorCabinetLekarstvoDto(
            d.Id,
            d.Quantity,
            d.CreateDate ?? DateTime.MinValue,
            d.DoctorCabinetId,
            $"{d.DoctorCabinet?.Account?.LastName} {d.DoctorCabinet?.Account?.FirstName} {d.DoctorCabinet?.Account?.SurName}" ?? "",
            d.PartiyaId,
            d.Partiya?.Lekarstvo?.Name ?? ""
            );
    }
}
