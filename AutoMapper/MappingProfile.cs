using AutoMapper;
using ExamNest.DTO.Authentication;
using ExamNest.DTO.Course;
using ExamNest.DTO.Exam;
using ExamNest.DTO.Question;
using ExamNest.DTO.Student;
using ExamNest.DTO.Submission;
using ExamNest.DTO.Track;
using ExamNest.Models;

namespace ExamNest.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BranchDTO, Branch>().ReverseMap();
            CreateMap<TrackDTO, Track>().ReverseMap();
            CreateMap<CourseDTO, Course>().ReverseMap();
            CreateMap<InstructorUpdateDto, Instructor>().ReverseMap();
            CreateMap<UpdateDto, Student>().ReverseMap();
            CreateMap<InstructorViewDto, Instructor>()
                .ReverseMap()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName))
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Track.TrackName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<StudentViewDto, Student>()
                .ReverseMap()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName))
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Track.TrackName))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<QuestionBankDTO, QuestionBank>().ReverseMap();
            CreateMap<ChoiceDTO, Choice>().ReverseMap();
            CreateMap<ExamDTO, Exam>().ReverseMap();
            CreateMap<SubmissionView, ExamSubmission>()
                .ReverseMap()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Student.User.Name))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Exam.Course.CourseName));


            CreateMap<ExamCreatePayload, ExamDTO>().ReverseMap();
            CreateMap<ExamUpdatePayloadDTO, ExamDTO>().ReverseMap();

            CreateMap<ExamSubmission, ExamSubmissionView>().ForMember(dest => dest.StudentName, orignal => orignal.MapFrom(src => src.Student.User.Name));



        }
    }
}
