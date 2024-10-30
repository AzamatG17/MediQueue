namespace MediQueue.Domain.Entities;

public class LekarstvoUsage : BaseUsage
{
    public int? ConclusionId { get; set; }
    public Conclusion? Conclusion { get; set; }

    public int? DoctorCabinetLekarstvoId { get; set; }
    public DoctorCabinetLekarstvo? DoctorCabinetLekarstvo { get; set; }
}
