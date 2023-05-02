using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  // la api se va a formar como // GET /api/users

    public class UsersController : ControllerBase
    {
        private readonly DataContext _context; //variable de clase de tipo Data context

        public UsersController(DataContext context) //constructor,instancia de DataContext, con esto tenemos disponible una session de la db
        {
            _context = context;
        }



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