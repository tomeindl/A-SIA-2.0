using Microsoft.AspNetCore.Mvc;
using A_SIA2WebAPI.Models;

namespace A_SIA2WebAPI.BL.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("{userId}")]
        public User GetUser([FromRoute] long userId)
        {
            return new User();
        }

        [HttpPost("Login")]
        public string Login([FromBody] CredentialsBody credentials)
        {
            if (credentials.Email != "admin@admin" || credentials.Password != "admin")
                Response.StatusCode = 401;
            return "";
        }

        [HttpPost("Logout")]
        public void Login()
        {

        }

        public class CredentialsBody
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        [HttpPost]
        public void Register([FromBody] User user)
        {

        }

        [HttpPut("{userId}")]
        public void UpdateUser([FromRoute] long userId, [FromBody] User user)
        {

        }

        [HttpDelete("{userId}")]
        public void DeleteUser([FromRoute] long userId)
        {

        }

        [HttpGet("{userId}/Networks")]
        public void GetUserNetworks([FromRoute] long userId)
        {

        }
    }
}
