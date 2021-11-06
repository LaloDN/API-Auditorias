using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        //IConfiguration me permite acceder al archivo appsettings.json
        private readonly IConfiguration _config;
        public LoginController(ILoginService loginService, IConfiguration config)
        {
            _loginService = loginService;
            _config = config;
        }

        //Endpoint para logear en el sistema
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginDTO usuario)
        {
            try
            {
                //Del objeto usuario, tomamos su password y la encriptamos, luego la actualizamos ahí mismo.
                usuario.Password = Encriptar.EncriptarPassword(usuario.Password);
                //Vamos a validar su existencia con su nombre de usuario y password encriptado.
                var user = await _loginService.ValidateUser(usuario);
                //Si el usuario es nulo, significa que el usuario no esta dentro de la base.
                if (user == null)
                {
                    //Le mandamos un mensaje al usuario indicandole que dicho usuario es inválido.
                    return BadRequest(new { message = "Usuario o contraseña invalidos" });
                }
                //Mandamos a llamar al método estático GetToken con el usario que vamos a conseguir su token
                //y una instancia para acceder al appsettings.json
                string tokenString = JWTConfigurator.GetToken(user, _config);
                //Retornamos el token en la petición
                return Ok(new { token=tokenString });
            }
            //Le enviamos un error por si algo sale mal.
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
