using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyetaV1.ViewModels
{
    public class AltaPerfilViewModel
    {
        public int Id { get; set; }
        public string DNI { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string CarreraProfesional { get; set; }
        public string Telefono { get; set; }
        public string Universidad { get; set; }
        public string Facultad { get; set; }
        public string FotoPerfil { get; set; }

    }
}
