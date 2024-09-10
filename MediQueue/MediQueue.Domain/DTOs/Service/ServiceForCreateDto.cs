using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.DTOs.Service
{
    public record ServiceForCreateDto(string Name, decimal Amount, int CategoryId);
}
