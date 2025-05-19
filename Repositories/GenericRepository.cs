using ExamNest.DTO.Authentication;
using ExamNest.Enums;
using ExamNest.Models;
using ExamNest.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExamNest.Repositories
{

    public abstract class GenericRepository
    {
        protected const int LimitPerPage = 10;
        protected IHttpContextAccessor HttpContextAccessor =>
            ServiceHelper.GetRequiredService<IHttpContextAccessor>();

        protected readonly AppDBContext AppDbContext;
        protected readonly IAppDBContextProcedures AppDbContextProcedures;
        public GenericRepository(AppDBContext appDb)
        {

            AppDbContext = appDb;
            AppDbContextProcedures = AppDbContext.GetProcedures();

        }


        protected int CalculatePagination(int page)
        {
            if (page < 1)
            {
                page = 1;
            }

            return (page - 1) * LimitPerPage;

        }

        protected async Task<CurrentUserDto> GetCurrentAsync()
        {

            var userId = HttpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var role = HttpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            switch (role)
            {

                case nameof(Roles.Student):
                    var student = await AppDbContext.Students.Include(student => student.User)
                                               .Include(student => student.Track).Include(student => student.Branch)
                                               .FirstOrDefaultAsync(u => u.UserId == userId);
                    return new StudentViewDto()
                    {
                        UserId = student.UserId,
                        Name = student.User.Name,
                        Email = student.User.Email,
                        TrackName = student.Track.TrackName,
                        BranchName = student.Branch.BranchName,
                        StudentId = student.StudentId,
                        role = role
                    };
                case nameof(Roles.Instructor):
                    var instructor = await AppDbContext.Instructors.Include(instructor => instructor.User)
                                                  .Include(instructor => instructor.Track)
                                                  .Include(instructor => instructor.Branch)
                                                  .FirstOrDefaultAsync(u => u.UserId == userId);
                    return new InstructorViewDto()
                    {
                        UserId = instructor.UserId,
                        Name = instructor.User.Name,
                        Email = instructor.User.Email,
                        TrackName = instructor.Track.TrackName,
                        BranchName = instructor.Branch.BranchName,
                        InstructorId = instructor.InstructorId,
                        role = role
                    };
                case nameof(Roles.Admin):
                    var admin = await AppDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    return new AdminViewDto()
                    {
                        UserId = admin.Id,
                        Name = admin.Name,
                        Email = admin.Email,
                        role = role
                    };
                case nameof(Roles.Pending):
                    var pending = await AppDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                    return new PendingViewDto()
                    {
                        UserId = pending.Id,
                        Name = pending.Name,
                        Email = pending.Email,
                        role = role

                    };
                default:
                    break;
            }


            throw new UnauthorizedAccessException();


        }



    }
}
