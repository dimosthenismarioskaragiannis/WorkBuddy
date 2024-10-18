using Microsoft.AspNetCore.Mvc;

namespace WorkBuddy.Api.Controllers
{
    //Using The ControllerBase cause the view part is on angular
    //I am creating a Base Api controller So my next controllers will derive from my base controller, instead of ControllerBase
    

    [ApiController]
    [Route("WorkBuddy.Api/[controller]")]
    public class BaseApiController:ControllerBase
    {


    }
}
