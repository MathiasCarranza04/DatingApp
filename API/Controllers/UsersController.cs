using System.Security.Claims;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize] //a alto nivel digo que todos los endpoints deben usar un token autorizado para consumirse
    public class UsersController : BaseApiController

    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper) //instancia de interfaz IUser
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }


        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() //la task representa una operacion async que retorna un value
        {
            var users = await _userRepository.GetMembersAsync(); //obtengo de la db los users porque _userRepository tiene el db context
            return Ok(users); //retorno de esos user lo especificado en el dto

        }

        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await _userRepository.GetMemberAsync(username);

        }


        [HttpPut] //no necesitamos el userName porque ya estoy autenticado para updatear mi user
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // NameIdentifier mapea con NameId que esta en el tokenService create token
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();
            _mapper.Map(memberUpdateDto, user); //updateo todas las propiedades que paso en memberupdatedto al user.sin guardar en db

            if (await _userRepository.SaveAllAsync()) return NoContent(); //actualizo en db
            return BadRequest("Fail to update user");

        }


    }
}