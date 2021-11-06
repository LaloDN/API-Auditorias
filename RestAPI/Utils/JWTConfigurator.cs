using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestAPI.Utils
{
    public class JWTConfigurator
    {
        /// <summary>
        /// Crea un JWT a partir de la información de login de un usuario.
        /// </summary>
        /// <param name="userInfo">Objeto Usuario con la información de inicio de sesión del usuario.</param>
        /// <param name="config">Instancia del appsettings.json</param>
        /// <returns>Una cadena con lae estructura de un JWT codificado con algoritmo HS256</returns>
        public static string GetToken(Usuario userInfo, IConfiguration config )
        {
            //Del archivo appsettings, tomo del objeto Jwt, tomo su propiedad Secretkey para obtener la clave para firmar.
            //los token
            string SecretKey = config["Jwt:SecretKey"];
            //Del archivo appsettings, tomo el issuer que tiene el dominio del front.
            string Issuer = config["Jwt:Issuer"];
            //Del archivo appsettings, tomo la udiencia que tiene el dominio del back.
            string Audience = config["Jwt:Audience"];

            //Esta parte es la securitykey del token, la generamos con la security key que tenemos en la variable SecretKey
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            //Eligimos el algoritmo con el que vamos a firman nuestras credenciales, en este caso con HS256
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Claims que va a tener nuestro token
            var claims = new[]
            {
                //Registraciones
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.NombreUsuario),
                //Tenemos un claim para el id de usuario, lo obtenemos gracias al usuario que le pasamos al método como parámetro.
                new Claim("idUsuario", userInfo.IdUsuario.ToString()),
                //En este otro claim tenemos el rol del usaurio que va a entrar
                new Claim("rol",userInfo.Rol.ToString())
            };

            //Configuración y creación de nuestro token
            var token = new JwtSecurityToken
            (
                //Issuer: el dominio del front
                issuer: Issuer,
                //Audience: dominio del back
                audience: Audience,
                //Claims que va a tener nuestro token a los que podremos accesar luego
                claims,
                //Tiempo de expiración del token.
                expires: DateTime.Now.AddMinutes(60),
                //Credenciales con las cuales vamos a firmar el token
                signingCredentials: credentials
            );
            //Retornamos el token que configuramos recientemente
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Obtiene el id de usuario de un JWT.
        /// </summary>
        /// <param name="identity">Objeto ClaimsIdentity con los claims del token</param>
        /// <returns>El id del usaurio dentro de la base de datos; -1 si ocurre un error.</returns>
        public static int TokenObtenerIdUsuario(ClaimsIdentity identity)
        {
            //Preguntamos si el identity es diferente de nulo para prevenir errores
            if (identity != null)
            {
                //Creamos un objeto de tipo Claim para poder iterar a traves de este,
                //le asignamos los claims de identity.
                IEnumerable<Claim> claims = identity.Claims;
                foreach(var claim in claims)
                {
                    //Pregutnamos el tipo de claim, nos interesa el de idUsuario,
                    //si lo encuentra, devuelve su valor en int
                    if (claim.Type == "idUsuario")
                    {
                        return int.Parse(claim.Value);
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Obtiene el rol de un JWT.
        /// </summary>
        /// <param name="identity">Objeto ClaimsIdentity con los claims del token.</param>
        /// <returns>El rol del usuario; "Error" si ocurre algún imprevisto.</returns>
        public static string TokenObtenerRol(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                foreach(var claim in claims)
                {
                    if (claim.Type == "rol")
                    {
                        return claim.Value;
                    }
                }
            }
            return "Error";
        }
    }
}
