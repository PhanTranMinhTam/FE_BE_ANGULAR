using Microsoft.EntityFrameworkCore;
using Test_Backend.Data;
using Test_Backend.Reponsitory;

namespace Test_Backend.Services
{
    public interface IStudentServices
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
    }
    public class StudentServices : IStudentServices
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public StudentServices(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _repositoryWrapper.Student.FindAll().ToListAsync();
        }
    }
}
