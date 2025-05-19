using ExamNest.Models;

namespace ExamNest.Factories
{
    public static class UserUpgradeFactory
    {
        public static Instructor CreateInstructor(string userId, int trackId, int branchId)
            => new Instructor { UserId = userId, TrackId = trackId, BranchId = branchId };

        public static Student CreateStudent(string userId, int trackId, int branchId)
            => new Student { UserId = userId, TrackId = trackId, BranchId = branchId };
    }

}
