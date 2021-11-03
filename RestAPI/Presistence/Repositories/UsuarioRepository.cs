using Microsoft.EntityFrameworkCore;
using RestAPI.Domain.IRepositories;
using RestAPI.Domain.Models;
using RestAPI.DTO;
using RestAPI.Presistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Presistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AplicationDbContext _context;
        public UsuarioRepository(AplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Agrega un usuario a la base de datos.
        /// </summary>
        /// <param name="usuario">Objeto Usuario a agregar en la tabla.</param>
        /// <returns>Una tarea guardando los cambios.</returns>
        public async Task SaveUser(Usuario usuario) 
        {
            _context.Add(usuario);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Revisa si un usuario existe o no dentro de la base por su nombre de usuario.
        /// </summary>
        /// <param name="usuario">Objeto Usuario a buscar en los registros.</param>
        /// <returns>True si encuentra un usuario dentro de la tabla; False en caso contrario.</returns>
        public async Task<bool> ValidateExistence(Usuario usuario)
        {
            //Con LINQ  busco si existe algún registro que concida con el NombreUsuario del usuario del parámetro,
            //con que encuentre una coincidencia se vuelve verdadero.
            var validateExistence = await _context.Usuarios.AnyAsync(x => x.NombreUsuario == usuario.NombreUsuario);
            return validateExistence;
        }

        /// <summary>
        /// Busca un usuario dentro de la tabla por su Id.
        /// </summary>
        /// <param name="idu">Id del usuario a buscar dentro de la tabla.</param>
        /// <returns>Un usuario dentro de la tabla Usuarios.</returns>
        public async Task<Usuario> ValidarPorId(int idu)
        {
            //Con LINQ hago una consulta, buscando el primer usuario que encuentre donde su IdUsuario 
            //coincida con el del parámetro idu.
            var user = await _context.Usuarios.Where(x => x.IdUsuario == idu).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// Valida si el password ingresado de un usuario es correcto.
        /// </summary>
        /// <param name="idUsuario">Id del usuario al que le vamos a cambiar el password.</param>
        /// <param name="passwordAnterior">Password actual del usuario.</param>
        /// <returns>Un usuario que coindica con el mismo IdUsuario y Password que le pasemos.</returns>
        public async Task<Usuario> ValidatePassword(int idUsuario, string passwordAnterior)
        {
            //Con LINQ, buscamos el registro dentro de la tabla Usuarios, el cual sus campos IdUsuario y Password coincidan 
            //con el de los parámetros
            var usuario = await _context.Usuarios.Where(x => x.IdUsuario == idUsuario && x.Password == passwordAnterior).FirstOrDefaultAsync();
            return usuario;

        }

        /// <summary>
        /// Actualiza la password de un usuario.
        /// </summary>
        /// <param name="usuario">Objeto Usuario al cuál le vamos a cambiar su password en la base.</param>
        /// <returns>Una tarea actualizando los cambios.</returns>
        public async Task UpdatePassword(Usuario usuario)
        {
            _context.Update(usuario);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Obtiene los registros dentro de la tabla Usuarios.
        /// </summary>
        /// <returns>Una lista de tipo Usuario con los registros en Usuarios.</returns>
        public async Task<List<Usuario>> ObtenerListaUsuarios()
        {
            var listaUsuarios = await _context.Usuarios.ToListAsync();
            return listaUsuarios;
        }

        /// <summary>
        /// Borra un usuario en la tabla Usuarios.
        /// </summary>
        /// <param name="usuario">Objeto Usuario a eliminar de nuestra base de datos.</param>
        /// <returns>Una tarea guardando los cambios realziados.</returns>
        public async Task BorrarUsuario(Usuario usuario)
        {
            _context.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
