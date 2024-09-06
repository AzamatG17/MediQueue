using MediQueue.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.Entities
{
    public class GroupCategory : EntityBase
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
