using RestAPI.Domain.IRepositories;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using RestAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task SaveUser(Usuario usuario)
        {
            await _usuarioRepository.SaveUser(usuario);
        }

        public async Task<bool> ValidateExistence(Usuario usuario)
        {
            return await _usuarioRepository.ValidateExistence(usuario);
        }

        public async Task<Usuario> ValidarPorId(int idu)
        {
            return await _usuarioRepository.ValidarPorId(idu);
        }


        public async Task<Usuario> ValidatePassword(int idUsuario, string passwordAnterior)
        {
            return await _usuarioRepository.ValidatePassword(idUsuario, passwordAnterior);
        }

        public async Task UpdatePassword(Usuario usuario)
        {
            await _usuarioRepository.UpdatePassword(usuario);
        }

        public async Task<List<Usuario>> ObtenerListaUsuarios()
        {
            return await _usuarioRepository.ObtenerListaUsuarios();
        }
        
        public async Task BorrarUsuario(Usuario usuario)
        {
            await _usuarioRepository.BorrarUsuario(usuario);
        }
    }
}
