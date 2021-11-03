using RestAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Domain.IRepositories
{
    public interface IDepartamentoRepository
    {
        public Task CrearDepartamento(Departamento departamento);
        public Task<bool> RevisarExistencia(Departamento departamento);

        public Task<List<Departamento>> ObtenerListaDepartamentos();

    }
}
