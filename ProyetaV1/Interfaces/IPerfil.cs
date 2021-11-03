using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyetaV1.Entities;

namespace ProyetaV1.Interfaces
{
    public  interface IPerfil:IRepository<Perfil>
    {
        Perfil AddPerfil(Perfil perfil);
        List<Perfil> GetAllPerfiles(Perfil perfil);
        Perfil GetPerfil(Perfil perfil);
        Perfil UpdatePerfil(Perfil perfil);

    }
}
