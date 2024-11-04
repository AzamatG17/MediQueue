using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class DoctorCabinetLekarstvo : EntityBase
{
    public decimal Quantity { get; set; }
    public DateTime? CreateDate { get; set; }
    public int DoctorCabinetId { get; set; }
    public DoctorCabinet DoctorCabinet { get; set; }
    public int PartiyaId { get; set; }
    public Partiya Partiya { get; set; }
    public virtual ICollection<LekarstvoUsage> LekarstvoUsages { get; set; }
}
