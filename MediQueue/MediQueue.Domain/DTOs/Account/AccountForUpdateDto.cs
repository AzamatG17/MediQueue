using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.DTOs.Account
{
    public record AccountForUpdateDto(
        int Id, string Login, string Password, string PhoneNumber, string FirstName, string LastName, string SurName, string Email, DateTime Birthdate, int RoleId);
}
