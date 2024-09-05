using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediQueue.Domain.Entities
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public Account User { get; set; }
    }
}
