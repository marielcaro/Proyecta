using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyetaV1.Entities
{
    public class AreaDeInteres
    {
        public int Id { get; set; }
        public Perfil Perfil { get; set; }
        public Disciplina Disciplina { get; set; }

    }
}
