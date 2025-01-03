using AutoMapper;
using MediQueue.Domain.DTOs.Sample;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Exceptions;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;

namespace MediQueue.Services;

public class SampleService : ISampleService
{
    public readonly ISampleRepository _repository;
    public readonly IMapper _mapper;

    public SampleService(ISampleRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<SampleDto>> GetAllSamplesAsync()
    {
        var samples = await _repository.FindAllAsync();

        if (samples == null) return null;

        return _mapper.Map<IEnumerable<SampleDto>>(samples);
    }

    public async Task<SampleDto> GetSampleByIdAsync(int id)
    {
        var sample = await _repository.FindByIdAsync(id)
            ?? throw new EntityNotFoundException($"Sample with id: {id} does not exist.");

        if (sample == null) return null;

        return _mapper.Map<SampleDto>(sample);
    }

    public async Task<SampleDto> CreateSampleAsync(SampleForCreateDto sampleForCreateDto)
    {
        ArgumentNullException.ThrowIfNull(nameof(sampleForCreateDto));

        var sample = _mapper.Map<Sample>(sampleForCreateDto);

        await _repository.CreateAsync(sample);

        return _mapper.Map<SampleDto>(sample);
    }

    public async Task<SampleDto> UpdateSampleAsync(SampleForUpdateDto sampleForUpdate)
    {
        ArgumentNullException.ThrowIfNull(sampleForUpdate);

        var sample = await _repository.FindByIdAsync(sampleForUpdate.Id);

        if (sample == null) throw new EntityNotFoundException($"Sample with Id: {sampleForUpdate.Id} does not exist !");

        sample.Name = sampleForUpdate.Name ?? "";
        sample.Description = sampleForUpdate.Description ?? "";

        await _repository.UpdateAsync(sample);

        return _mapper.Map<SampleDto>(sample);
    }

    public async Task DeleteSampleAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
