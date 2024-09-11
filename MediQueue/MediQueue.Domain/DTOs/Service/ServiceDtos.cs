using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.DTOs.Service
{
    public record ServiceDtos(int id, string Name, decimal Amount, int CategoryId, string CategoryName);
}