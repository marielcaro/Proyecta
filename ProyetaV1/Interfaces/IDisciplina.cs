using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyetaV1.Entities;

namespace ProyetaV1.Interfaces
{
    public interface IDisciplina: IRepository<Disciplina>
    {
        Disciplina AddDisciplina(Disciplina disciplina);
        List<Disciplina> GetAllDisciplinas();
        Disciplina GetDisciplina(Disciplina disciplina);
        Disciplina UpdateDisciplina(Disciplina disciplina);
    }
}
