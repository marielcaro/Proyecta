using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyetaV1.Entities;

namespace ProyetaV1.ViewModels
{
    public class AltaDisciplinaViewModel
    {
        public int Id { get; set; }
        public string NombreDisciplina { get; set; }
        public ICollection<AreaDeInteres> AreaDeInteres { get; set; }
    }
}
