using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyetaV1.Entities;

namespace ProyetaV1.ViewModels
{
    public class IdPerfilViewModel
    {
        public int Id { get; set; }
        public Perfil Perfil { get; set; }

        public string Email { get; set; }
    }
}
