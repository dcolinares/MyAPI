using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MyAPI.Application;
using MyAPI.Model;
using MyAPI.Repositories;
using System.Drawing.Printing;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private readonly Service _service;
        public UserController(Service service)
        {
            _service = service;
        }
        [HttpGet("get")]
        public IActionResult GetUser([FromQuery]string userName, [FromQuery]string password)
        {
            var user = _service.User.GetUser(userName, password);
            if (user == null)
            {
                return NotFound();
            } else 
            {
                _service.User.UpdateUserToken(user);
                return Ok(user);
            }
            
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel user)
        {
            var result = await _service.User.SaveCreateUser(user);
            return Ok(result);

        }

        [HttpPost("update-password")]
        public IActionResult UpdateUserPassword([FromQuery] int userID, [FromQuery] string password)
        {
            var result = _service.User.ResetPassword(userID, password);
            if (result != "")
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
