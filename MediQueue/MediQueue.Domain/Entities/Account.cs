using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Account : EntityBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public DateTime Bithdate { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
