using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProyetaV1.Entities
{
    public class User: IdentityUser
    {
        public bool IsActive { get; set; }
        public string UserType { get; set; }
    }
}
