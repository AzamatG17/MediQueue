using MediQueue.Domain.Entities.Responses;

namespace MediQueue.Domain.DTOs.DoctorCabinetLekarstvo;

public record DoctorCabinetLekarstvoForCreateDto(
    int DoctorCabinetId,
    List<DoctorCabinetResponse> DoctorCabinetResponses
    );
