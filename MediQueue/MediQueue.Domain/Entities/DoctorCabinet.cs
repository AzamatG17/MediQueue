using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class DoctorCabinet : EntityBase
{
    public string? RoomNumber { get; set; }
    public int? AccountId { get; set; }
    public virtual Account? Account { get; set; }

    public virtual ICollection<DoctorCabinetLekarstvo>? DoctorCabinetLekarstvos { get; set; }
}
