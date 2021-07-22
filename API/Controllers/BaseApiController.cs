using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // 1. Manages the DTOs, you could use a dto as request params (post), to map what was sent
    //    To your DTO
    // 2. Automatically validates the parameters that we pass up to an api endpoind based on validation
    //    we set
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}