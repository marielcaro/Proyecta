using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyetaV1.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastSession { get; set; }
        public string UserType { get; set; }
        public bool Verified {get; set;}
        public Perfil Perfil { get; set; }

    }
}
