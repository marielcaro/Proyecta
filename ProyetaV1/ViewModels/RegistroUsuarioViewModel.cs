using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyetaV1.Entities;

namespace ProyetaV1.ViewModels
{
    public class RegistroUsuarioViewModel
    {

        public int Id { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public DateTime CreationDate { get; set; }
        public Perfil Perfil { get; set; }


    }
}
