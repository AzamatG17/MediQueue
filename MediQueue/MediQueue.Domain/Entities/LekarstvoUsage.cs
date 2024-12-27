namespace MediQueue.Domain.Entities;

public class LekarstvoUsage : BaseUsage
{
    public int? ConclusionId { get; set; }
    public virtual Conclusion? Conclusion { get; set; }

    public int? DoctorCabinetLekarstvoId { get; set; }
    public virtual DoctorCabinetLekarstvo? DoctorCabinetLekarstvo { get; set; }
}
