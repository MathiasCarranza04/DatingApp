using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize] //a alto nivel digo que todos los endpoints deben usar un token autorizado para consumirse
    public class UsersController : BaseApiController

    {
        private readonly DataContext _context; //variable de clase de tipo Data context

        public UsersController(DataContext context) //constructor,instancia de DataContext, con esto tenemos disponible una session de la db
        {
            _context = context;
        }



        [AllowAnonymous] //especifico que el getUsers no necesita bearer token para usarse, esto no es seguro
        [HttpGet]

        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() //la task representa una operacion async que retorna un value
        {
            var users = await _context.Users.ToListAsync(); //del contexto de la session de la db retorno toda la lista de users
            return users;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {

            var user = await _context.Users.FindAsync(id);
            return user;
        }

    }
}