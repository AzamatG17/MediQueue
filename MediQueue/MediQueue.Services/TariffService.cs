using AutoMapper;
using MediQueue.Domain.DTOs.Tariff;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class TariffService : ITariffService
{
    private readonly ITariffRepository _repository;
    private readonly IMapper _mapper;

    public TariffService(ITariffRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<TariffDto>> GetAllTariffsAsync()
    {
        var tariffs = await _repository.FindAllAsync();

        if (tariffs == null) return null;

        return _mapper.Map<IEnumerable<TariffDto>>(tariffs);
    }

    public async Task<TariffDto> GetTariffByIdAsync(int id)
    {
        var tariff = await _repository.FindByIdAsync(id);

        if (tariff == null) return null;

        return _mapper.Map<TariffDto>(tariff);
    }

    public async Task<TariffDto> CreateTariffAsync(TariffForCreateDto tariffForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(tariffForCreateDto));

        var tariff = _mapper.Map<Tariff>(tariffForCreateDto);

        await _repository.CreateAsync(tariff);

        return _mapper.Map<TariffDto>(tariff);
    }

    public async Task<TariffDto> UpdateTariffAsync(TariffForUpdateDto tariffForUpdateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(tariffForUpdateDto));

        var tariff = await _repository.FindByIdAsync(tariffForUpdateDto.Id)
            ?? throw new KeyNotFoundException($"Tariff with id: {tariffForUpdateDto.Id} does not exist.");

        tariff.Name = tariffForUpdateDto.Name;
        tariff.PricePerDay = tariffForUpdateDto.PricePerDay;

        await _repository.UpdateAsync(tariff);

        return _mapper.Map<TariffDto>(tariff);
    }

    public async Task DeleteTariffAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
