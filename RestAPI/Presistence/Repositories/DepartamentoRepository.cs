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
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly AplicationDbContext _context;


        public DepartamentoRepository(AplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Agrega un objeto Departamento a la base de datos.
        /// </summary>
        /// <param name="departamento">Objeto Departamento parseado de un JSON</param>
        /// <returns>Una tarea guardando el departamento</returns>
        public async Task CrearDepartamento(Departamento departamento)
        {
            _context.Add(departamento);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Valida si ya existe un departamento con el mismo nombre dentro de la base de datos.
        /// </summary>
        /// <param name="departamento">Objeto Departamento parseado de un JSON.</param>
        /// <returns>True si ya existe el departamento; False en caso contrario.</returns>
        public async Task<bool> RevisarExistencia(Departamento departamento)
        {
            //Preguntamos si ya existe un departamento con el mismo nombre dentro de la base con LINQ
            var existence = await _context.Departamentos.AnyAsync(x => x.NombreDepartamento == departamento.NombreDepartamento);
            return existence;
        }

        /// <summary>
        /// Obtiene los registros dentro de la tabla Departamento en la base.
        /// </summary>
        /// <returns>Una lista de tipo Departamento con los registros.</returns>
        public async Task<List<Departamento>> ObtenerListaDepartamentos()
        {
            var listaDepartamentos = await _context.Departamentos.ToListAsync();
            return listaDepartamentos;
        }

    }
}
