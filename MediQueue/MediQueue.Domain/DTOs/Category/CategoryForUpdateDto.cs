using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.DTOs.Category
{
    public record CategoryForUpdateDto(int Id, string CategoryName, List<int> GroupIds);
}
