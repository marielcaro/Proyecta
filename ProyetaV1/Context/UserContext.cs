using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProyetaV1.Entities;
using Microsoft.EntityFrameworkCore;


namespace ProyetaV1.Context
{
    public class UserContext: IdentityDbContext<User>
    {
        private const string Schema = "users";
        public UserContext()
        {

        }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(connectionString: "Data Source = (localdb)\\MSSQLLocalDB; Database = UsersDb; Integrated Security = True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Fluent dpi
            //No hay ningún db set
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(Schema);
        }
    }
}
