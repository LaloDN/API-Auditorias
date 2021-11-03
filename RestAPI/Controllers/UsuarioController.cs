using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using RestAPI.DTO;
using RestAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        //Endpoint para agregar a un usuario al sistema.
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            try
            {
                //Validamos si el usuario ya existe con el método ValidateExistence pasandole
                //el usuario que vamos a agregar como parámetro.
                var validateExistence = await _usuarioService.ValidateExistence(usuario);
                //Si validateExistence es true, es que el usuario ya existe.
                if (validateExistence)
                {
                    //Le enviamos un mensaje indicando que el usuario ya existe.
                    return BadRequest(new { message = "El usuario " + usuario.NombreUsuario + " ya existe!" });
                }
                //Encriptamos el password del usuario antes de guardarlo en la base.
                usuario.Password = Encriptar.EncriptarPassword(usuario.Password);
                //Guardamos el usuario con el método SaveUser
                await _usuarioService.SaveUser(usuario);
                //Le enviamos un mensaje confirmando la operación
                return Ok(new { message = "Usuario registrado con exito!" });
            }//Enviamos un error por si sale algo mal.
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Endpoint para cambiar el password de un usuario
        [Route("CambiarPassword")]
        [HttpPut]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDTO cambiarPassword)
        {
            try
            {
                int idUsuario = 13;
                string passwordEncriptado = Encriptar.EncriptarPassword(cambiarPassword.passwordAnterior);
                var usuario = await _usuarioService.ValidatePassword(idUsuario, passwordEncriptado);
                if (usuario == null)
                {
                    return BadRequest(new { message = "La password es incrorrecta" });
                }
                else
                {
                    usuario.Password = Encriptar.EncriptarPassword(cambiarPassword.nuevaPassword);
                    await _usuarioService.UpdatePassword(usuario);
                    return Ok(new { message = "La password fue actualizada con exito!" });

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Endpoint para obtener la lista de los usuarios dentro de la base.
        [HttpGet]
        public async Task<List<Usuario>> Get()
        {
            return await _usuarioService.ObtenerListaUsuarios();   
        }

        //Endpoint para borrar un usuario en la base, mediante un objeto IdUsuarioDTO
        [HttpDelete]
        public async Task<IActionResult> Delete(IdUsuarioDTO idUsuarioDTO)
        {
            try
            {
                //Extraemos el id del usuario que vamos a borrar del objeto idUsuarioDTO
                int idu = idUsuarioDTO.idUsuario;
                //Ese id se lo pasamos al método ValidarPorId para ver si existe
                var user = await _usuarioService.ValidarPorId(idu);
                //Si user es null, significa que el usuario no existe dentro de la base.
                if (user==null)
                {
                    //Le mandamos un mensaje indicandole que no existe el usuario que indicó.
                    return BadRequest(new { message = "El usuario no existe!" });
                }
                //Si el usuario no es null, seguimos con el proceso invocando el método BorrarUsuario.
                    await _usuarioService.BorrarUsuario(user);
                //Enviamos un mensaje confirmando la operación.
                    return Ok(new { message="El usuario se ha borrado con éxito!"});
                
            }//Enviamos un error por si algo sale mal.
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
