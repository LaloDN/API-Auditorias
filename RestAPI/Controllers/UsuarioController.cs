using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Domain.ISerivces;
using RestAPI.Domain.Models;
using RestAPI.DTO;
using RestAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
                //Guardamos el usuario con el método SaveUser y obtenemos el id con el que se guarda.
                int id = await _usuarioService.SaveUser(usuario);
                //Le enviamos un mensaje confirmando la operación
                return Ok(new { message = "El usuario " + usuario.NombreUsuario + " se ha registrado con el id " + id });
            }//Enviamos un error por si sale algo mal.
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Endpoint para cambiar el password de un usuario
        [Route("CambiarPassword")]
        [HttpPut]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CambiarPassword([FromBody] CambiarPasswordDTO cambiarPassword)
        {
            try
            {
                //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //Invoco al método que me va a traer exclusivamente el id del usuario logeado, pasandole el identity
                int idUsuario = JWTConfigurator.TokenObtenerIdUsuario(identity);
                //Encripto la password para enivarla a comparar en la base de datos
                string passwordEncriptado = Encriptar.EncriptarPassword(cambiarPassword.passwordAnterior);
                //Llamada al método para validar que el usuario y la password esten correctos
                var usuario = await _usuarioService.ValidatePassword(idUsuario, passwordEncriptado);
                if (usuario == null)
                {
                    return BadRequest(new { message = "La password es incrorrecta" });
                }
                else
                {
                    //Encripto la nueva password y se la asigno al campo de password del usuario al que voy a cambiarsela.
                    usuario.Password = Encriptar.EncriptarPassword(cambiarPassword.nuevaPassword);
                    //Mandamos a llamar el método para actualizar el password. 
                    await _usuarioService.UpdatePassword(usuario);
                    //Mandamos un mensaje confirmando la operación
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
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<Usuario>> Get()
        {
            //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
            string rol = JWTConfigurator.TokenObtenerRol(identity);
            if (rol == "RH")
            {
                //Si el usuario es un RH retornamos toda la lista de usuarios
                return await _usuarioService.ObtenerListaUsuarios();
            }
            else
            {
                //Si el usuario no es un RH, le enviamos una lista con único usuario inválido-
                List<Usuario> Error = new List<Usuario> { new Usuario { IdUsuario = -1, NombreUsuario = "Error", Password = "####", Rol = "Error" } };
                return Error;
            }
        }

        //Endpoint para borrar un usuario en la base, mediante un objeto IdUsuarioDTO
        [HttpDelete("id={id}")]
        //Protección del endpoint
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                //En este objeto, con este HttpContext, me va a traer TODOS LOS ATRIBUTOS DEL TOKEN, incluidos los claims.
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                //Invoco al método que me va a traer exclusivamente el rol del usuario logeado, pasandole el identity.
                string rol = JWTConfigurator.TokenObtenerRol(identity);
                //Preguntamos si el usuario es un RH, si lo es, entramos al proceso de borrar un usuario.
                if (rol == "RH")
                {
                    //El id que tenemos en el url se lo pasamos al método ValidarPorId para ver si existe el usuario a eliminar
                    var user = await _usuarioService.ValidarPorId(id);
                    //Si user es null, significa que el usuario no existe dentro de la base.
                    if (user == null)
                    {
                        //Le mandamos un mensaje indicandole que no existe el usuario que indicó.
                        return BadRequest(new { message = "El usuario no existe!" });
                    }
                    //Obtenemos el id del usuario que esta llamando a este método, el cuál debe ser un RH
                    int idUsuarioRH = JWTConfigurator.TokenObtenerIdUsuario(identity);
                    //Necesitamos preguntar si el id del usuario actual es diferente al del que vamos a borrar.
                    if (idUsuarioRH!=id) {
                        //Si el usuario no es null, seguimos con el proceso invocando el método BorrarUsuario.
                        await _usuarioService.BorrarUsuario(user);
                        //Enviamos un mensaje confirmando la operación.
                        return Ok(new { message = "El usuario "+ user.NombreUsuario+ " ha sido borrado con éxito!" });
                    }
                    else
                    {
                        //Si el usuario quiere borrarse a si mismo, le mandamos un error
                        return BadRequest(new { message = " No puede borrarse a si mismo. " });
                    }
                }
                else
                {
                    //Si no es un RH, le mandamos un error
                    return BadRequest(new { message = "No tiene permiso de ejecutar esta operación" });
                }
            }//Enviamos un error por si algo sale mal.
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
