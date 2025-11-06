using Microsoft.AspNetCore.Mvc;
using Test_Backend.Authorization;
using Test_Backend.Data;
using Test_Backend.Models;
usin Test_Backend.Services;
namespace Test_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly MyDbContext _dbContext;
        public UserController(IUserServices userServices, MyDbContext dbContext)
        {
            _userServices = userServices;
            _dbContext = dbContext;

        }
        [Permisstion("Read")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {

            IEnumerable<User> users = await _userServices.GetAllUsersAsync();

            if (users == null || !users.Any())
            {
                return BadRequest(new
                {
                    Status = "Error",
                    Message = "No users found."
                });
            }

            return Ok(new
            {
                Status = "Success",
                Data = users
            });
        }
    }
}