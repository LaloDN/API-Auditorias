using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Domain.IRepositories;
using RestAPI.Domain.ISerivces;
using RestAPI.Presistence.Context;
using RestAPI.Presistence.Repositories;
using RestAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AplicationDbContext>(options =>
             options.UseMySql(Configuration.GetConnectionString("Conexion")));

            //Services
            services.AddScoped<IPreguntaService, PreguntaService>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            services.AddScoped<IDepartamentoService, DepartamentoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ILoginService, LoginService>();

            //Repository
            services.AddScoped<IPreguntaRepository, PreguntaRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();

            //Cors
            services.AddCors(options => options.AddPolicy("AllowWebapp",
                                                builder => builder.AllowAnyOrigin() //Le digo que permita cualquier origen de las peticiones
                                                                .AllowAnyMethod()   //Le digo que puedo utilizar cualquiera de los métodos
                                                                .AllowAnyHeader())); //Le digo que me permita utilizar cualquier cabezera

            //Autenticación del token
            //Aquí le digo que esquema de autenticación  voy a utilizar
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //Aquí dentro van a ir todas las cosas que quiero que se vaiden cuando yo ingreso un token
                    //Validamos el issuer, o sea, el dominio del front
                    ValidateIssuer = true,
                    //Validamos la audiencia, el dominio del back.
                    ValidateAudience = true,
                    //Validamos si el token sigue activo
                    ValidateLifetime = true,
                    //Validamos la security key
                    ValidateIssuerSigningKey = true,
                    //Obtenemos el dominio del front
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    //Obtenemos el dominio del back
                    ValidAudience=Configuration["Jwt:Audience"],
                    //Obtenemos la secret key del appsettings
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                    //Timespan
                    ClockSkew=TimeSpan.Zero
                }) ;
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Con esta linea de código le digo que utilize el cors que cree en los servicios.
            app.UseCors("AllowWebapp");
            //Con esta línea de código le digo que utilize la autenticación que configure en los serviocios.
            app.UseAuthentication();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
