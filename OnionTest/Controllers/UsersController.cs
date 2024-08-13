using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionTest.Application.Services;
using OnionTest.Contracts;

namespace OnionTest.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IResult> Register([FromBody] UserRegisterRequest request)
        {
            await _userService.Register(request.username, request.email, request.password);
            return Results.Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IResult> Login([FromBody] UserLoginRequest request)
        {
            var token = await _userService.Login(request.email, request.password);

            Response.Cookies.Append("tasty-cookie", token["access"]);
            Response.Cookies.Append("tastiest-cookie", token["refresh"]);

            Dictionary<string, object> user = new()
            {
                { "Id", Convert.ToInt32(token["Id"]) },
                { "UserName", token["UserName"]}
            };

            return Results.Ok(user);
        }

        [Authorize(Policy = "user")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            
            string userName = await _userService.GetUserById(id);
            return Ok(userName);
            
        }


        [AllowAnonymous]
        [HttpGet("GetUserByIdFromCookie")]
        public async Task<IResult> GetUserByIdFromCookie()
        {
            var token = Request.Cookies["tasty-cookie"];

            if(token==null)
                return Results.Unauthorized();

            string userName = await _userService.GetUsernameFromToken(token);

            return Results.Ok(userName);
        }

        [AllowAnonymous]
        [HttpPost("UserExistByEmail")]
        public async Task<IResult> UserExistByEmail([FromBody] UserRegisterRequest request)
        {
            return Results.Ok(await _userService.UserExistsByEmail(request.email));
        }
    }
}
