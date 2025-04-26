using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ExamNest.DTO
{
    public class InstructorViewDTO
    {
        public int BranchId { get; set; }
        public int TrackId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string BranchName { get; set; }
        public string TrackName {  get; set; }

    }
}
