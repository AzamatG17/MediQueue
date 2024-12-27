using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities;

public class Account : EntityBase
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Passport { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string SurName { get; set; }
    public string PhotoBase64 { get; set; }
    public DateTime Bithdate { get; set; }

    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
    public int? DoctorCabinetId { get; set; }
    public virtual DoctorCabinet? DoctorCabinet { get; set; }

    public virtual ICollection<QuestionnaireHistory> QuestionnaireHistories { get; set; }
    public virtual ICollection<RolePermission> RolePermissions { get; set; }
    public virtual ICollection<Service> Services { get; set; }
}  
