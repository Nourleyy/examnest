using ExamNest.Interfaces;
using ExamNest.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Repositories
{
    public interface IStudentRepository : IGeneric<Student>
    {
        Task<StudentView?> GetUserById(string currentUser);
    }
   public class StudentView
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string TrackName { get; set; }
        public string BranchName { get; set; }
        public int StudentId { get; set; }
    }
    public class StudentRepository : GenericRepository, IStudentRepository
    {
        public StudentRepository(AppDBContext appDB) : base(appDB)
        {
        }

        public async Task<StudentView?> GetUserById(string currentUser)
        {
            var user = await _appDBContext.Students
                .Include(u => u.User)
                .Include(u => u.Track)
                .Include(u => u.Branch)
                    .Select(u => new StudentView
                    {
                        UserId = u.UserId,
                        Name = u.User.Name,
                        Email = u.User.Email,
                        TrackName = u.Track.TrackName,
                        BranchName = u.Branch.BranchName,
                        StudentId = u.StudentId
                    })
                .FirstOrDefaultAsync(u => u.UserId == currentUser);

            return user;
        }

        public async Task<decimal?> Create(Student entityDto)
        {
            var createdStudentResult = await appDBContextProcedures.CreateStudentAsync(entityDto.BranchId,entityDto.TrackId,entityDto.UserId);
            if (createdStudentResult.FirstOrDefault() == null)
            {
                throw new InvalidOperationException("Student not created");
            }
            return createdStudentResult.FirstOrDefault()?.StudentID;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int?> Update(int id, Student entity)
        {
            throw new NotImplementedException();
        }
    }
}
