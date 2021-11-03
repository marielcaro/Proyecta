using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyetaV1.Entities;

namespace ProyetaV1.Interfaces
{
    public interface IAreaDeIntereses:IRepository<AreaDeInteres>
    {
        AreaDeInteres AddAreaDeInteres(AreaDeInteres areaDeInteres);
        List<AreaDeInteres> GetAllAreasDeInteres();
        AreaDeInteres GetAreaDeInteres(AreaDeInteres areaDeInteres);
        AreaDeInteres UpdateAreaDeInteres(AreaDeInteres areaDeInteres);
    }
}
