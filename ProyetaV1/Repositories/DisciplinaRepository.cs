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
    public class DisciplinaRepository: BaseRepository<Disciplina, ProyectaContext>, IDisciplina
    {
        public DisciplinaRepository(ProyectaContext dbContext) : base(dbContext)
        {

        }

        public Disciplina AddDisciplina(Disciplina disciplina)
        {
            return Add(disciplina);
        }

        public List<Disciplina> GetAllDisciplinas()
        {
            return GetAllEntities();
        }

        public Disciplina GetDisciplina(Disciplina disciplina)
        {
            return Get(disciplina.Id);
        }

        public Disciplina UpdateDisciplina(Disciplina disciplina)
        {
            return Update(disciplina);
        }
    }
}
