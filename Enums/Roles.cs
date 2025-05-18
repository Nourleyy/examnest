namespace ExamNest.Enums
{

    public enum Roles
    {
        Admin,
        Instructor,
        Student
    }
     static class Permissions
    {
        public const string AddBranch = "Permissions.AddBranch";
        public const string UpdateBranch = "Permissions.UpdateBranch";
        public const string DeleteBranch = "Permissions.DeleteBranch";
        public const string CreateCourse = "Permissions.CreateCourse";
        public const string EditCourse = "Permissions.EditCourse";
        public const string DeleteCourse = "Permissions.DeleteCourse";
        public const string CreateExam = "Permissions.CreateExam";
        public const string EditExam = "Permissions.EditExam";
        public const string DeleteExam = "Permissions.DeleteExam";
        public const string ViewExam = "Permissions.ViewExam";
        public const string CreateQuestion = "Permissions.CreateQuestion";
        public const string EditQuestion = "Permissions.EditQuestion";
        public const string DeleteQuestion = "Permissions.DeleteQuestion";
        public const string CreateUser = "Permissions.CreateUser";
        public const string EditUser = "Permissions.EditUser";
        public const string DeleteUser = "Permissions.DeleteUser";
        public const string AddSubmission = "Permissions.AddSubmission";
        public const string GetSubmissions = "Permissions.GetAllSubmissions";
        public const string GetSubmission = "Permissions.GetSubmission";
        public const string GetSubmissionDetails = "Permissions.GetSubmissionDetails";
        public const string DeleteSubmission = "Permissions.DeleteSubmission";

    }
    public static class RolePermissions
    {
        public static IReadOnlyList<string> Admin => new List<string>
        {
            Permissions.AddBranch,
            Permissions.UpdateBranch,
            Permissions.DeleteBranch,
            Permissions.CreateCourse,
            Permissions.EditCourse,
            Permissions.DeleteCourse,
            Permissions.CreateExam,
            Permissions.EditExam,
            Permissions.DeleteExam,
            Permissions.ViewExam,
            Permissions.CreateQuestion,
            Permissions.EditQuestion,
            Permissions.DeleteQuestion,
            Permissions.CreateUser,
            Permissions.EditUser,
            Permissions.DeleteUser,
            Permissions.AddSubmission,
            Permissions.GetSubmissions,
            Permissions.GetSubmission,
            Permissions.GetSubmissionDetails,
            Permissions.DeleteSubmission
        };

        public static IReadOnlyList<string> Instructor => new List<string>
        {
            Permissions.CreateCourse,
            Permissions.EditCourse,
            Permissions.CreateExam,
            Permissions.EditExam,
            Permissions.ViewExam,
            Permissions.CreateQuestion,
            Permissions.EditQuestion,
            Permissions.AddSubmission,
            Permissions.GetSubmissionDetails
        };

        public static IReadOnlyList<string> Student => new List<string>
        {
            Permissions.AddSubmission,
            Permissions.GetSubmission,
            Permissions.GetSubmissionDetails
        };
    }
}