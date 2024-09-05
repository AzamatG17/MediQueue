using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.DTOs.Account
{
    public record AccountForCreateDto(string login, string password, string phoneNum, string FisrtName, string LastName, DateTime Bithdate, int RoleId);
}
