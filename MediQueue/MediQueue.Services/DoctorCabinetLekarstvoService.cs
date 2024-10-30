using MediQueue.Domain.DTOs.DoctorCabinet;
using MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;
using MediQueue.Domain.Entities;
using MediQueue.Domain.Interfaces.Repositories;
using MediQueue.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Services;

public class DoctorCabinetLekarstvoService : IDoctorCabinetService
{
    public readonly IDoctorCabinetRepository _repository;

    public DoctorCabinetLekarstvoService(IDoctorCabinetRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<DoctorCabinetDto>> GetAllDoctorCabinetsAsync()
    {
        var doctorCabinets = await _repository.FindAllAsync();

        if (doctorCabinets == null) return null;

        return doctorCabinets.Select(MapToDoctorCabinetDto).ToList();
    }

    public Task<DoctorCabinetDto> GetDoctorCabinetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorCabinetDto> CreateDoctorCabinetAsync(DoctorCabinetForCreate doctorCabinetForCreate)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorCabinetDto> UpdateDoctorCabinetAsync(DoctorCabinetForUpdate doctorCabinetForUpdate)
    {
        throw new NotImplementedException();
    }

    public Task DeleteDoctorCabinetAsync(int id)
    {
        throw new NotImplementedException();
    }

    private DoctorCabinetDto MapToDoctorCabinetDto(DoctorCabinet d)
    {
        return new DoctorCabinetDto(
            d.Id,
            d.RoomNumber,
            d.AccountId,
            $"{d.Account?.LastName} {d.Account?.FirstName} {d.Account?.SurName}" ?? "",
            d.DoctorCabinetLekarstvos != null
                ? d.DoctorCabinetLekarstvos.Select(MapToDoctorCabinetLekarstvoDto).ToList()
                : new List<DoctorCabinetLekarstvoDto>()
            );
    }

    private DoctorCabinetLekarstvoDto MapToDoctorCabinetLekarstvoDto(DoctorCabinetLekarstvo d)
    {
        return new DoctorCabinetLekarstvoDto(
            d.Id,
            d.Quantity,
            d.DoctorCabinetId,
            $"{d.DoctorCabinet?.Account?.LastName} {d.DoctorCabinet?.Account?.FirstName} {d.DoctorCabinet?.Account?.SurName}" ?? "",
            d.PartiyaId
            );
    }
}
