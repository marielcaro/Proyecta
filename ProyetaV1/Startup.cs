using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyetaV1.Context;
using ProyetaV1.Entities;
using ProyetaV1.Repositories;
using ProyetaV1.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;



namespace ProyetaV1
{
    public class Startup
    {
        //Agregado de CROS - para que se comunique con el Frontend
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*",
                                                          "*");
                                  });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProyetaV1", Version = "v1" });

                //Incorporo el uso del token dentro del swagger
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Ingrese Bearer [Token] para poder identificarse dentro de la aplicación"


                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                          {
                                 Reference= new OpenApiReference
                                 {
                                        Type=ReferenceType.SecurityScheme,
                                        Id="Bearer"
                                 }
                          },
                        new List<string>()
                    }
            });

                //Añade un tipo de autentificación:
                /*Agrega autentificacion JWT ("Con "Bearers"")*/
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true; // que guarde el toke na autorizado
                        options.RequireHttpsMetadata = false; //no necesito aun https
                        options.TokenValidationParameters = new TokenValidationParameters
                        { //parametros para la validacion del token
                            ValidateIssuer = true, //entidad que genera el token
                            ValidateAudience = true, //los que usan o ven el token
                            ValidAudience = "https://localhost:5001",
                            ValidIssuer = "https://localhost:5001",
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySecretaSuperLargaDeAutorizacion")) //Encargado de proveernos info con la llave secreta para ingresar a la aplicación
                                                                                                                                      // TOKEN = HEADER + PILOT + FIRMA
                        };
                    });

                // Conectar la Base de Datos
                services.AddEntityFrameworkSqlServer();
                services.AddDbContextPool<ProyectaContext>(optionsAction: (provider, builder) =>
                {
                    builder.UseInternalServiceProvider(provider);
                    builder.UseSqlServer(connectionString: "Data Source=(localdb)\\MSSQLLocalDB;Database=ProyectaDb;Integrated Security=True;");
                });

                // Inyección de dependencia de repositorios:
                services.AddScoped<IUser, UserRepository>();
                services.AddScoped<IPerfil, PerfilRepository>();
                services.AddScoped<IDisciplina, DisciplinaRepository>();
                services.AddScoped<IAreaDeIntereses, AreaDeInteresRepository>();

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProyetaV1 v1"));
                }


                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseCors(MyAllowSpecificOrigins);

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }

        //CORREGIR AQUÍ
        // AGREGAR EN LA BD OTROS DOS CAMPOS LLAMADOS TOKENDESESION Y VALIDOTO 
        //AGREGAR EL LOGIN RESPONSE VIEW MODEL

        private async Task<LoginResponseViewModel> GetToken(User currentUser)
        {
            var userRoles = await _userManager.GetRolesAsync(currentUser);

            var authClaims = new List<Claim>()
            {
                new Claim (ClaimTypes.Name,currentUser.UserName),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //agregamos anuestra lista de privilegios o claims todos los privilegios de nuestro usuario
            // ESTOS PRIVILEGIOS  SON LOS DE TIPO DE USUARIO DE LA BD
            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            //levantamos nuestro signin key
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySecretaSuperLargaDeAutorizacion")); //Encargado de proveernos info con la llave secreta para ingresar a la aplicación

            //creo el token
            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256));

            return new LoginResponseViewModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };
        }
    }
    }

