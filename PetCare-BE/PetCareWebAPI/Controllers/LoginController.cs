using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using PetCareWebAPI.DAL;
using PetCareWebAPI.DAL.Entities;
using PetCareWebAPI.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using PetCareWebAPI.Response;

namespace PetCareWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginServices _services;
        private readonly IConfiguration _config;

        public LoginController(LoginServices services, IConfiguration config)
        {
            _services = services;
            _config = config;
        }

        /// <summary> 
        /// Valida si la persona existe y se le asigna el valor del token asociado al usuario
        /// </summary> 
        /// <param name="person">DTO con Email y Contraseña</param> 
        /// <returns>ResponseLogin</returns>
        [HttpPost]
        public async Task<ActionResult<ResponseLogin>> Login(LogInDTO person)
        {
            ResponseLogin responseLogin = new ResponseLogin();
            //Revisamos si la persona existe en la bd
            var persona = await _services.GetPerson(person);

            if (persona == null)
            {
                responseLogin.response = "Datos erroneos"; 
                responseLogin.status = "Error";
                return Ok(responseLogin);
            }
            
            //Generamos tokens
            responseLogin.response = GenerateToken(persona);
            if(responseLogin.response != null)
            {
                responseLogin.status = "ok";
                responseLogin.idUser = persona.Identification.ToString();
            }
            else
                return Ok();

            return Ok(responseLogin);
        }

        /// <summary> 
        /// Genera token asociado al Nombre, Email y tipo de Usuario
        /// </summary> 
        /// <param name="person">DTO con todos los datos de la persona</param> 
        /// <returns>Tokens</returns>
        private string GenerateToken(Person person)
        {
            //Creamos claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, person.Name),
                new Claim(ClaimTypes.Email, person.Email),
                new Claim("userType", person.userType.ToString())
            };

            //Configuramos con que schemas se deben de configurar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.Aes128CbcHmacSha256);

            //Creamos el token en base a lo que se configuro anteriormente
            var securityToken = new JwtSecurityToken(
                                claims: claims,
                                expires: DateTime.Now.AddMinutes(300000),
                                signingCredentials: credenciales
                                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken); //Escribimos el token para retornarlo

            return token;
        }
    }
}
