using Microsoft.EntityFrameworkCore;
using RestAPI.Domain.IRepositories;
using RestAPI.Domain.Models;
using RestAPI.Presistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Presistence.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly AplicationDbContext _context;
        public LoginRepository(AplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Valida si un usuario existe dentro de la base de datos.
        /// </summary>
        /// <param name="usuario">Objeto Usuario que queremos validar.</param>
        /// <returns>Un usuario que coincida con el objeto usuario recibido.</returns>
        public async Task<Usuario> ValidateUser(Usuario usuario)
        {
            //Mediante una consulta LINQ, le decimos que encuentre el primer registro dentro de mi tabla
            //Usuarios que coincida con el nombre de usuario y el password de mi objeto usuario del parámetro.
            var user = await _context.Usuarios.Where(x => x.NombreUsuario == usuario.NombreUsuario
                                                && x.Password == usuario.Password).FirstOrDefaultAsync();
            return user;
        }
    }
}
