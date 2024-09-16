using MediQueue.Domain.Common;

namespace MediQueue.Domain.Entities
{
    public class Sclad : EntityBase
    {
        public string Name { get; set; }

        public int Branchid { get; set; }
        public Branch Branch { get; set; }
        public virtual ICollection<Lekarstvo> Lekarstvos { get; set; }
    }
}
