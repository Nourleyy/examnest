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
        }
    }
}
