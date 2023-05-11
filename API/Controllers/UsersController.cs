using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    }
}