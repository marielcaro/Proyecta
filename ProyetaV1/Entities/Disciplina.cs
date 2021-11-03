using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyetaV1.Entities
{
    public class Disciplina
    {
        public int Id { get; set; }
        public string NombreDisciplina { get; set; }
        public ICollection<AreaDeInteres> AreaDeInteres { get; set; }

    }
}
