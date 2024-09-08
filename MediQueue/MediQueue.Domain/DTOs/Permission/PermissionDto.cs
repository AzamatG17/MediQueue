using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.DTOs.Permission
{
    public record PermissionDto(int Id, string Name, string Controller, string Action);
}
