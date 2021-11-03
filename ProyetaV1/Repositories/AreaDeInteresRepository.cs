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
    public class AreaDeInteresRepository: BaseRepository<AreaDeInteres, ProyectaContext>, IAreaDeIntereses
    {
        public AreaDeInteresRepository(ProyectaContext dbContext) : base(dbContext)
        {

        }

        public AreaDeInteres AddAreaDeInteres(AreaDeInteres areaDeInteres)
        {
            return Add(areaDeInteres);
        }

        public List<AreaDeInteres> GetAllAreasDeInteres()
        {
            return GetAllEntities();
        }

        public AreaDeInteres GetAreaDeInteres(AreaDeInteres areaDeInteres)
        {
            return Get(areaDeInteres.Id);
        }

        public AreaDeInteres UpdateAreaDeInteres(AreaDeInteres areaDeInteres)
        {
            return Update(areaDeInteres);
        }
    }
}
