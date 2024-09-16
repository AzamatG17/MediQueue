using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Branch : EntityBase
    {
        public string Name { get; set; }
        public string? Addres { get; set; }

        public virtual ICollection<Sclad> Sclads { get; set; }
    }
}
