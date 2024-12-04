using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;

public record DoctorCabinetLekarstvoForUpdateDto(
    int Id,
    int? DoctorCabinetId,
    List<DoctorCabinetResponse> DoctorCabinetResponses
    );
