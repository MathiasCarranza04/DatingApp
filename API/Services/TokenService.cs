using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key; //key para encriptar la data

        public TokenService(IConfiguration config) //inyeccion de dependencia en el constructor
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));  // este TokenKey es el nombre que voy a guardar en la configuracion en dev.json, el token key luego tendra su valor
        }


        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.UserName) // afirmo ser el user.UserName, es tipo valor
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);  //aca firmo el token por security purpose

            var tokenDescriptor = new SecurityTokenDescriptor //nuestro token va a incluir lo siguiente
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), //nuestro token expire en 7 dias
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler(); //objecto usado para crear/escribir el token, es el que hace algo con el token

            var token = tokenHandler.CreateToken(tokenDescriptor); //creo el token, usando todo lo que tiene el token descriptor

            return tokenHandler.WriteToken(token);  // retorno el token en formato string

        }
    }
}