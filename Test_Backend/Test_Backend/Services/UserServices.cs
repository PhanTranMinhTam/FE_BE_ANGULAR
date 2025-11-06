using Microsoft.EntityFrameworkCore;
using Test_Backend.Data;
using Test_Backend.Reponsitory;

namespace Test_Backend.Services
{
    public interface IUserServices
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        //Task<User> GetUserByIdAsync(int id);
    }
    public class UserServices :  IUserServices
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        //private readonly IMapper _mapper;
        public UserServices(IRepositoryWrapper repositoryWrapper)
        {
            //_mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            
            return await _repositoryWrapper.User.FindAll().ToListAsync();
        }
    }
}
