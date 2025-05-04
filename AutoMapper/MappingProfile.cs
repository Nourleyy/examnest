using AutoMapper;
using ExamNest.DTO;
using ExamNest.Models;

namespace ExamNest.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<BranchDTO,Branch>().ReverseMap();
            CreateMap<TrackDTO,Track>().ReverseMap();
            CreateMap<CourseDTO,Course>().ReverseMap();
            CreateMap<UserDTO,Instructor>().ReverseMap();
            CreateMap<UserViewDTO, Instructor>()
                .ReverseMap()
                .ForMember(dest => dest.BranchName, opt=> opt.MapFrom(src=>src.Branch.BranchName))
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Track.TrackName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<UserViewDTO, Student>()
                .ReverseMap()
                .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.BranchName))
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Track.TrackName))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
            CreateMap<QuestionBankDTO, QuestionBank>().ReverseMap();
            CreateMap<ChoiceDTO, Choice>().ReverseMap();
            CreateMap<ExamDTO, Exam>().ReverseMap();
            CreateMap<SubmissionDTO, ExamSubmission>()
                .ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Student.User.Name))
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Exam.Course.CourseName));

        }
    }
}
