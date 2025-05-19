
namespace ExamNest.DTO.Authentication
{
    public class CurrentUserDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string role { get; set; }

    }

    public class StudentViewDto : CurrentUserDto
    {
        public string TrackName { get; set; }
        public string BranchName { get; set; }
        public int StudentId { get; set; }
    }

    public class InstructorViewDto : CurrentUserDto
    {
        public string TrackName { get; set; }
        public string BranchName { get; set; }
        public int InstructorId { get; set; }
    }

    public class AdminViewDto : CurrentUserDto
    {
    }

    public class PendingViewDto : CurrentUserDto
    {
    }
}