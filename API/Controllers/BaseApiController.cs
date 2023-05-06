using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController] //necesario para que se puedan hacer http request en todos los controllers
    [Route("api/[controller]")]  // esta linea determina como se va a formar el nombre de la api

    public class BaseApiController : ControllerBase
    {

    }
}