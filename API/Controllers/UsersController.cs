using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) //instancia de interfaz IUser
        {
            _photoService = photoService;
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


        [HttpPut] //no necesitamos el {userName} del get de arriba porque ya estoy autenticado para updatear mi user
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.GetUsername(); //obtengo el username de la extension Claims
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();
            _mapper.Map(memberUpdateDto, user); //updateo todas las propiedades que paso en memberupdatedto al user.sin guardar en db

            if (await _userRepository.SaveAllAsync()) return NoContent(); //actualizo en db
            return BadRequest("Fail to update user");

        }


        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUsername(); //obtengo el nombre del user authenticado con su claim
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            var result = await _photoService.AddPhotoAsync(file); //subo la foto a Cloudinary
            if (result.Error != null) return BadRequest(result.Error.Message);

            //creo foto usando la entity para photos
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0) photo.IsMain = true; //si es la primera foto del usuario sera la main

            user.Photos.Add(photo); //asocio photo al user

            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetUser), new { username = user.UserName }, _mapper.Map<PhotoDto>(photo)); //si nuestro cambios se guardaron en db retorno un photodto from the photo )
            }
            return BadRequest("Problem adding photo."); // si no entro al if anterior algo no se subio bien
        }


    }
}