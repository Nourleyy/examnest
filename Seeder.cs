using Bogus;
using ExamNest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExamNest
{
    public class SeedCounts
    {
        public int Instructors { get; set; } = 150;
        public int Students { get; set; } = 500;
        public int Branches { get; set; } = 5;
        public int TracksPerBranchMin { get; set; } = 3;
        public int TracksPerBranchMax { get; set; } = 5;
        public int QuestionsPerCourseMin { get; set; } = 50;
        public int QuestionsPerCourseMax { get; set; } = 60;
        public int ExamsPerCourseMin { get; set; } = 1;
        public int ExamsPerCourseMax { get; set; } = 5;
        public int ExamQuestionsMin { get; set; } = 20;
        public int ExamQuestionsMax { get; set; } = 50;
        public double StudentExamTakenRateMin { get; set; } = 0.7;
        public double StudentExamTakenRateMax { get; set; } = 0.9;
    }

    public static class DataSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider, SeedCounts counts)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<AppDBContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await context.Database.MigrateAsync();

                if (context.Users.Any())
                {
                    return;
                }

                await SeedRolesAsync(roleManager);
                var users = await SeedUsersAsync(userManager, counts);

                var branches = await SeedBranchesAsync(context, counts);
                var tracks = await SeedTracksAsync(context, branches, counts);
                var courses = await SeedCoursesAsync(context, tracks);

                var instructorUsers = users.Where(u => u.Item2 == "Instructor").Select(u => u.Item1).ToList();
                var studentUsers = users.Where(u => u.Item2 == "Student").Select(u => u.Item1).ToList();

                var instructors = await SeedInstructorsAsync(context, branches, tracks, instructorUsers);
                var students = await SeedStudentsAsync(context, branches, tracks, studentUsers);
                var questions = await SeedQuestionBankAsync(context, courses, counts);
                var exams = await SeedExamsAsync(context, courses, counts);

                await SeedExamQuestionsAsync(context, exams, questions, counts);

                await SeedExamSubmissionsAsync(context, exams, students, questions, counts);

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Instructor", "Student", "Guest" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task<List<Tuple<User, string>>> SeedUsersAsync(
            UserManager<User> userManager, SeedCounts counts)
        {
            var result = new List<Tuple<User, string>>();
            var password = "Password123!";

            var admin = new User
            {
                UserName = "admin@examnest.com",
                Email = "admin@examnest.com",
                Name = "Admin User",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(admin.Email) == null)
            {
                await userManager.CreateAsync(admin, password);
                await userManager.AddToRoleAsync(admin, "Admin");
                result.Add(new Tuple<User, string>(admin, "Admin"));
            }

            var instructorFaker = new Faker<User>()
                                  .RuleFor(u => u.UserName,
                                           f => f.Internet.Email(f.Name.FirstName(), f.Name.LastName(), "examnest.com")
                                                 .ToLower())
                                  .RuleFor(u => u.Email, (f, u) => u.UserName)
                                  .RuleFor(u => u.Name, f => f.Name.FullName())
                                  .RuleFor(u => u.EmailConfirmed, true)
                                  .RuleFor(u => u.PhoneNumberConfirmed, true);

            var instructors = instructorFaker.Generate(counts.Instructors);
            foreach (var instructor in instructors)
            {
                if (await userManager.FindByEmailAsync(instructor.Email) == null)
                {
                    await userManager.CreateAsync(instructor, password);
                    await userManager.AddToRoleAsync(instructor, "Instructor");
                    result.Add(new Tuple<User, string>(instructor, "Instructor"));
                }
            }

            var studentFaker = new Faker<User>()
                               .RuleFor(u => u.UserName,
                                        f => f.Internet
                                              .Email(f.Name.FirstName(), f.Name.LastName(), "student.examnest.com")
                                              .ToLower())
                               .RuleFor(u => u.Email, (f, u) => u.UserName)
                               .RuleFor(u => u.Name, f => f.Name.FullName())
                               .RuleFor(u => u.EmailConfirmed, true)
                               .RuleFor(u => u.PhoneNumberConfirmed, true);

            var students = studentFaker.Generate(counts.Students);
            foreach (var student in students)
            {
                if (await userManager.FindByEmailAsync(student.Email) == null)
                {
                    await userManager.CreateAsync(student, password);
                    await userManager.AddToRoleAsync(student, "Student");
                    result.Add(new Tuple<User, string>(student, "Student"));
                }
            }

            return result;
        }

        private static async Task<List<Branch>> SeedBranchesAsync(AppDBContext context, SeedCounts counts)
        {
            var branchNames = new[] { "Cairo", "Alexandria", "Giza", "Aswan", "Luxor" };
            var branches = new List<Branch>();

            foreach (var name in branchNames.Take(counts.Branches))
            {
                var branch = new Branch { BranchName = name };
                context.Branches.Add(branch);
                branches.Add(branch);
            }

            await context.SaveChangesAsync();
            return branches;
        }

        private static async Task<List<Track>> SeedTracksAsync(AppDBContext context, List<Branch> branches,
                                                               SeedCounts counts)
        {
            var trackNames = new[]
                             {
                                 "Software Engineering", "Data Science", "Cybersecurity",
                                 "Cloud Computing", "Artificial Intelligence", "Mobile Development", "Web Development"
                             };

            var faker = new Faker();
            var tracks = new List<Track>();

            foreach (var branch in branches)
            {
                int count = faker.Random.Int(counts.TracksPerBranchMin, counts.TracksPerBranchMax);
                var selected = faker.Random.Shuffle(trackNames).Take(count);
                foreach (var name in selected)
                {
                    var track = new Track
                    {
                        BranchId = branch.BranchId,
                        TrackName = name
                    };
                    context.Tracks.Add(track);
                    tracks.Add(track);
                }
            }

            await context.SaveChangesAsync();
            return tracks;
        }

        private static async Task<List<Course>> SeedCoursesAsync(AppDBContext context, List<Track> tracks)
        {
            var coursesByTrack = new Dictionary<string, string[]>
            {
                ["Software Engineering"] =
                                         new[]
                                         {
                                             "OOP", "Design Patterns", "Software Testing", "Agile Development",
                                             "Project Management"
                                         },
                ["Data Science"] = new[]
                                                        {
                                                            "Data Analysis", "Machine Learning", "Big Data",
                                                            "Statistical Computing", "Data Visualization"
                                                        },
                ["Cybersecurity"] = new[]
                                                         {
                                                             "Network Security", "Ethical Hacking",
                                                             "Cryptography", "Risk Management", "Forensics"
                                                         },
                ["Cloud Computing"] = new[]
                                                           {
                                                               "Cloud Architecture", "Virtualization",
                                                               "Containerization", "Cloud Integration",
                                                               "Serverless"
                                                           },
                ["Artificial Intelligence"] =
                                         new[]
                                         {
                                             "NLP", "Computer Vision", "Deep Learning", "AI Ethics",
                                             "Reinforcement Learning"
                                         },
                ["Mobile Development"] =
                                         new[] { "iOS", "Android", "Cross-platform", "Mobile UX", "App Testing" },
                ["Web Development"] = new[]
                                                           {
                                                               "Frontend", "Backend", "Full-Stack",
                                                               "Web Security", "Performance Optimization"
                                                           }
            };

            var courses = new List<Course>();

            foreach (var track in tracks)
            {
                if (coursesByTrack.TryGetValue(track.TrackName, out var courseNames))
                {
                    foreach (var name in courseNames)
                    {
                        var course = new Course
                        {
                            TrackId = track.TrackId,
                            CourseName = name
                        };
                        context.Courses.Add(course);
                        courses.Add(course);
                    }
                }
            }

            await context.SaveChangesAsync();
            return courses;
        }

        private static async Task<List<Instructor>> SeedInstructorsAsync(
            AppDBContext context, List<Branch> branches, List<Track> tracks, List<User> instructorUsers)
        {
            var faker = new Faker();
            var instructors = new List<Instructor>();

            foreach (var user in instructorUsers)
            {
                var branch = faker.Random.ListItem(branches);
                var branchTracks = tracks.Where(t => t.BranchId == branch.BranchId).ToList();
                if (!branchTracks.Any()) continue;
                var track = faker.Random.ListItem(branchTracks);

                var instructor = new Instructor
                {
                    BranchId = branch.BranchId,
                    TrackId = track.TrackId,
                    UserId = user.Id
                };
                context.Instructors.Add(instructor);
                instructors.Add(instructor);
            }

            await context.SaveChangesAsync();
            return instructors;
        }

        private static async Task<List<Student>> SeedStudentsAsync(AppDBContext context, List<Branch> branches,
                                                                   List<Track> tracks, List<User> studentUsers)
        {
            var faker = new Faker();
            var students = new List<Student>();

            foreach (var user in studentUsers)
            {
                var branch = faker.Random.ListItem(branches);
                var branchTracks = tracks.Where(t => t.BranchId == branch.BranchId).ToList();
                if (!branchTracks.Any()) continue;
                var track = faker.Random.ListItem(branchTracks);

                var student = new Student
                {
                    BranchId = branch.BranchId,
                    TrackId = track.TrackId,
                    UserId = user.Id
                };
                context.Students.Add(student);
                students.Add(student);
            }

            await context.SaveChangesAsync();
            return students;
        }

        private static async Task<List<QuestionBank>> SeedQuestionBankAsync(
            AppDBContext context, List<Course> courses, SeedCounts counts)
        {
            var faker = new Faker();
            var questions = new List<QuestionBank>();

            foreach (var course in courses)
            {
                int qCount = faker.Random.Int(counts.QuestionsPerCourseMin, counts.QuestionsPerCourseMax);
                for (int i = 0; i < qCount; i++)
                {
                    var question = new QuestionBank
                    {
                        CourseId = course.CourseId,
                        QuestionText = faker.Lorem.Sentence(),
                        QuestionType = "MCQ",
                        ModelAnswer = faker.Random.ArrayElement(new[] { "A", "B", "C", "D" }),
                        Points = faker.Random.Int(1, 5)
                    };
                    context.QuestionBanks.Add(question);
                    await context.SaveChangesAsync();

                    for (char letter = 'A'; letter <= 'D'; letter++)
                    {
                        var choice = new Choice
                        {
                            QuestionId = question.QuestionId,
                            ChoiceLetter = letter.ToString(),
                            ChoiceText = faker.Lorem.Sentence()
                        };
                        context.Choices.Add(choice);
                    }

                    questions.Add(question);
                }
            }

            await context.SaveChangesAsync();
            return questions;
        }

        private static async Task<List<Exam>> SeedExamsAsync(AppDBContext context, List<Course> courses,
                                                             SeedCounts counts)
        {
            var faker = new Faker();
            var exams = new List<Exam>();

            foreach (var course in courses)
            {
                int examCount = faker.Random.Int(counts.ExamsPerCourseMin, counts.ExamsPerCourseMax);
                for (int i = 0; i < examCount; i++)
                {
                    var examDate = faker.Date.Recent(30);
                    var endDate = examDate.AddHours(faker.Random.Int(1, 3));

                    var exam = new Exam
                    {
                        CourseId = course.CourseId,
                        ExamDate = examDate,
                        EndDate = endDate
                    };
                    context.Exams.Add(exam);
                    exams.Add(exam);
                }
            }

            await context.SaveChangesAsync();
            return exams;
        }

        private static async Task SeedExamQuestionsAsync(AppDBContext context, List<Exam> exams,
                                                         List<QuestionBank> allQuestions, SeedCounts counts)
        {
            var faker = new Faker();
            foreach (var exam in exams)
            {
                var courseQuestions = allQuestions.Where(q => q.CourseId == exam.CourseId).ToList();
                if (!courseQuestions.Any()) continue;

                int count = Math.Min(faker.Random.Int(counts.ExamQuestionsMin, counts.ExamQuestionsMax),
                                     courseQuestions.Count);
                var selected = faker.Random.Shuffle(courseQuestions).Take(count);
                foreach (var question in selected)
                {
                    context.Set<ExamQuestion>().Add(new ExamQuestion
                    {
                        ExamId = exam.ExamId,
                        QuestionId = question.QuestionId
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedExamSubmissionsAsync(AppDBContext context, List<Exam> exams,
                                                           List<Student> students, List<QuestionBank> questions,
                                                           SeedCounts counts)
        {
            var faker = new Faker();
            foreach (var exam in exams)
            {
                var course = await context.Courses.FindAsync(exam.CourseId);
                if (course == null) continue;

                var trackStudents = students.Where(s => s.TrackId == course.TrackId).ToList();
                if (!trackStudents.Any()) continue;

                var examQuestionIds = await context.Set<ExamQuestion>()
                                                   .Where(eq => eq.ExamId == exam.ExamId)
                                                   .Select(eq => eq.QuestionId)
                                                   .ToListAsync();
                if (!examQuestionIds.Any()) continue;

                int takeCount = (int)(trackStudents.Count *
                                      faker.Random.Double(counts.StudentExamTakenRateMin,
                                                          counts.StudentExamTakenRateMax));
                var selectedStudents = faker.Random.Shuffle(trackStudents).Take(takeCount);

                foreach (var student in selectedStudents)
                {
                    var submission = new ExamSubmission
                    {
                        ExamId = exam.ExamId,
                        StudentId = student.StudentId,
                        SubmissionDate = faker.Date.Between(exam.ExamDate, exam.EndDate)
                    };
                    context.ExamSubmissions.Add(submission);
                    await context.SaveChangesAsync();

                    int totalPoints = 0;
                    int earnedPoints = 0;

                    foreach (var qId in examQuestionIds)
                    {
                        var question = await context.QuestionBanks.FindAsync(qId);
                        if (question == null) continue;
                        totalPoints += question.Points;
                        bool isCorrect = faker.Random.Double() < faker.Random.Double(0.6, 0.9);
                        var givenAnswer = isCorrect ? question.ModelAnswer : GetRandomWrongAnswer(question.ModelAnswer);
                        if (isCorrect)
                            earnedPoints += question.Points;

                        context.StudentAnswers.Add(new StudentAnswer
                        {
                            SubmissionId = submission.SubmissionId,
                            QuestionId = qId,
                            StudentAnswer1 = givenAnswer
                        });
                    }

                    if (totalPoints > 0)
                    {
                        submission.Score = (decimal)earnedPoints / totalPoints * 100;
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        private static string GetRandomWrongAnswer(string correctAnswer)
        {
            var faker = new Faker();
            var options = new[] { "A", "B", "C", "D" };
            return faker.Random.ArrayElement(options.Where(o => o != correctAnswer).ToArray());
        }
    }
}