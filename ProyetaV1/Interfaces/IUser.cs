using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyetaV1.Entities;

namespace ProyetaV1.Interfaces
{
    public interface IUser: IRepository<User>
    {
        User AddUser(User user);
        List<User> GetAllUsers();
        User GetUser(User user);
        User UpdateUser(User user);

    }
}
