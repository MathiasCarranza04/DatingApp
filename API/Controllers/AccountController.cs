using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context; // class variable

        private readonly ITokenService _tokenService;

        public AccountController(DataContext context, ITokenService tokenService) //inyecto el context en el constructor
        {
            _tokenService = tokenService;
            _context = context;

        }



        [HttpPost("register")]  // creo endpoint con ruta >> //api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName)) return BadRequest("UserName is taken");

            using var hmac = new HMACSHA512(); //instancio HMAC para acceder a passwords hasheados

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), //hasheo el pass que llega del user front end
                PasswordSalt = hmac.Key

            };

            _context.Users.Add(user); // añado user al context
            await _context.SaveChangesAsync(); //inserto el user en db

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName); //si el user que me llega desde fe(dto) esta en la db es valido
            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);  //instancio HMAC para poder buscar el passwordSalt del user hasheado en la db

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password)); // hash calculado a partir del password que me llega del loginDto

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password"); // si el hash calculado no es igual al hash que tengo en db doy error
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}