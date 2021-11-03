using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProyetaV1.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public List<T> GetAllEntities();
        public T Get(int id);
        public T Add(T entity);
        public T Update(T entity);
        public T Delete(int id);
    }
}
