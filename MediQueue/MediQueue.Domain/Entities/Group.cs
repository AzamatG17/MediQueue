using MediQueue.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.Entities
{
    public class Group : EntityBase
    {
        public string GroupName { get; set; }

        public ICollection<GroupCategory> GroupCategories { get; set; }
    }
}
