using Microsoft.EntityFrameworkCore.Storage;
using Test_Backend.Data;

namespace Test_Backend.Reponsitory
{
    public interface IStudentRepository : IRepositoryBase<Student> { }
    public interface IRepositoryWrapper
    {
        IStudentRepository Student { get; }
        void Save();
        Task SaveAsync();
        IDbContextTransaction Transaction();
    }

    public class StudentRepository : ReponsitoryBase<Student>, IStudentRepository
    {
        public StudentRepository(MyDbContext context) : base(context) { }
    }
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private IStudentRepository Student;
        private readonly MyDbContext context;

        public RepositoryWrapper(MyDbContext context)
        {
            this.context = context;
        }

        public IStudentRepository Student => student ??= new StudentRepository(context);
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
