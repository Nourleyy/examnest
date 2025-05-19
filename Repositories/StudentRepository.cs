using AutoMapper;
using ExamNest.DTO.Authentication;
using ExamNest.Interfaces;
using ExamNest.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Repositories
{
    public class StudentRepository : GenericRepository, IStudentRepository
    {
        private readonly IMapper mapper;
        public StudentRepository(AppDBContext appDB, IMapper mapper) : base(appDB)
        {
            this.mapper = mapper;
        }



        public async Task<decimal?> Create(Student entityDto)
        {
            var createdStudentResult = await AppDbContextProcedures.CreateStudentAsync(entityDto.BranchId, entityDto.TrackId, entityDto.UserId);
            if (createdStudentResult.FirstOrDefault() == null)
            {
                throw new InvalidOperationException("Student not created");
            }
            return createdStudentResult.FirstOrDefault()?.StudentID;
        }

        public async Task<bool> Delete(int id)
        {


            var deleteById = await AppDbContext.Students.Where(s => s.StudentId == id).ExecuteDeleteAsync();
            return deleteById > 0;
        }

        public async Task<List<StudentViewDto>> GetAll(int page)
        {
            var students = await AppDbContext.Students
                                   .Include(t => t.Track)
                                   .Include(b => b.Branch)
                                   .Include(u => u.User)
                                   .Take(LimitPerPage)
                                      .Skip(CalculatePagination(page))
                                   .ToListAsync();

            return mapper.Map<List<StudentViewDto>>(students);

        }

        public async Task<StudentViewDto> GetById(int id)
        {
            var student = await AppDbContext.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            return mapper.Map<StudentViewDto>(student);
        }

        public async Task<StudentViewDto?> GetStudentByUserId(string userId)
        {
            var result = await AppDbContext.Students.FirstOrDefaultAsync(s => s.UserId == userId);

            return result == null ? null : mapper.Map<StudentViewDto>(result);
        }

        public async Task<int?> Update(int id, Student entity)
        {
            var result = await AppDbContextProcedures.UpdateStudentAsync(entity.StudentId, entity.BranchId, entity.TrackId, entity.UserId);
            if (result.FirstOrDefault() == null)
            {
                throw new InvalidOperationException("Student not updated");
            }
            return result.FirstOrDefault()?.RowsUpdated;
        }
    }
}
