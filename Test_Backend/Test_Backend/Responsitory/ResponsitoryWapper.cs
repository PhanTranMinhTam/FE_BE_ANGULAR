using Microsoft.EntityFrameworkCore.Storage;
using Test_Backend.Data;

namespace Test_Backend.Reponsitory
{
    public interface IStudentRepository : IRepositoryBase<Student> { }
    public interface IUserRepository : IRepositoryBase<User> { }
    public interface IAuthReponsitory : IRepositoryBase<Models.AuthDTO> { }
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken> { }
    public interface IRolePermisstionRepository : IRepositoryBase<Rolepermission> { }
    public interface IRepositoryWrapper
    {
        IStudentRepository Student { get; }
        IUserRepository User { get; }
        IRolePermisstionRepository RolePermisstion { get; }
        IAuthReponsitory Auth { get; }
        IRefreshTokenRepository RefreshToken { get; }
        void Save();
        Task SaveAsync();
        IDbContextTransaction Transaction();
    }
    public class UserRepository : ReponsitoryBase<User>, IUserRepository
    {
        public UserRepository(MyDbContext context) : base(context) { }
    }
    public class AuthReponsitory : ReponsitoryBase<Models.AuthDTO>, IAuthReponsitory
    {
        public AuthReponsitory(MyDbContext context) : base(context) { }
    }
    public class StudentRepository : ReponsitoryBase<Student>, IStudentRepository
    {
        public StudentRepository(MyDbContext context) : base(context) { }
    }
    public class RolePermisstionRepository : ReponsitoryBase<Rolepermission>, IRolePermisstionRepository
    {
        public RolePermisstionRepository(MyDbContext context) : base(context) { }
    }
    public class RefreshTokenRepository : ReponsitoryBase<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(MyDbContext context) : base(context) { }
    }
    public class RepositoryWrapper : IRepositoryWrapper
    {
        //private readonly MyDbContext _context;
        private IStudentRepository _student;
        private IRolePermisstionRepository RolePermisstionRepository;
        private IUserRepository user;
        private IAuthReponsitory AuthRepository;
        private IRefreshTokenRepository RefreshRepository;
        private readonly MyDbContext context;
        public RepositoryWrapper(MyDbContext context)
        {
            this.context = context;
        }

        public IRolePermisstionRepository RolePermisstion => RolePermisstionRepository ??= new RolePermisstionRepository(context);
        public IUserRepository User => user ??= new UserRepository(context);
        public IAuthReponsitory Auth => AuthRepository ??= new AuthReponsitory(context);
        public IStudentRepository Student => _student ??= new StudentRepository(context);

        public IRefreshTokenRepository RefreshToken => RefreshRepository ??= new RefreshTokenRepository(context);
        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public IDbContextTransaction Transaction()
        {
            return context.Database.BeginTransaction();
        }
    }
}
