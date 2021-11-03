using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProyetaV1.Entities;
using ProyetaV1.Context;
using ProyetaV1.Interfaces;
using ProyetaV1.Repositories;

namespace ProyetaV1.Repositories
{
    public class PerfilRepository: BaseRepository<Perfil,ProyectaContext>, IPerfil
    {
        public PerfilRepository(ProyectaContext dbContext) : base(dbContext)
        {

        }

        public Perfil AddPerfil(Perfil perfil)
        {
            return Add(perfil);
        }

        public List<Perfil> GetAllPerfiles(Perfil perfil)
        {
            return GetAllEntities();
        }

        public Perfil GetPerfil(Perfil perfil)
        {
            return Get(perfil.Id);
        }

        public Perfil UpdatePerfil(Perfil perfil)
        {
            return Update(perfil);
        }
    }
}
