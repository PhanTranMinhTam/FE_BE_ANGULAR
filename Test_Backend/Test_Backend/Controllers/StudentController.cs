using Microsoft.AspNetCore.Mvc;
using Test_Backend.Data;
using Test_Backend.Services;
using Test_Backend.Authorization;
namespace Test_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServices _studentServices;
        private readonly MyDbContext _dbContext;
        public StudentController(IStudentServices studentServices, MyDbContext dbContext)
        {
            _studentServices = studentServices;
            _dbContext = dbContext;

        }
        [Permisstion("ViewUser")]
        [HttpGet]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            IEnumerable<Student> students = await _studentServices.GetAllStudentsAsync();

            if (students == null || !students.Any())
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
                Data = students
            });
        }
    }
}
