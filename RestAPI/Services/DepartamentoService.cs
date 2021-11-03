using RestAPI.Domain.IRepositories;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Services
{
    public class DepartamentoService : IDepartamentoService
    {
        private readonly IDepartamentoRepository _departamentoRepository;

        public DepartamentoService(IDepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        public async Task CrearDepartamento(Departamento departamento)
        {
            await _departamentoRepository.CrearDepartamento(departamento);
        }

        public async Task<bool> RevisarExistencia(Departamento departamento)
        {
            return await _departamentoRepository.RevisarExistencia(departamento);
        }

        public async Task<List<Departamento>> ObtenerListaDepartamentos()
        {
            return await _departamentoRepository.ObtenerListaDepartamentos();
        }
    }
}
