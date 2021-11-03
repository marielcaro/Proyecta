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
    public class UserRepository: BaseRepository<User, ProyectaContext>, IUser
    {
        public UserRepository(ProyectaContext dbContext) : base(dbContext)
        {

        }

        public User AddUser(User user)
        {
            return Add(user);
        }

        public List<User> GetAllUsers()
        {
            return GetAllEntities();
        }

        public User GetUser(User user)
        {
            return Get(user.Id);
        }

        public User UpdateUser(User user)
        {
            return Update(user);
        }
    }
}
