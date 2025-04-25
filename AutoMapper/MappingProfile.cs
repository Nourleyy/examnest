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
            CreateMap<CreateTrackDTO,Track>().ReverseMap();
            //CreateMap<TrackDTO, Track>()
            //    .ReverseMap()
            //    .ForMember(dest => dest.BranchName,
            //    opt => opt.MapFrom(src => src.Branch.BranchName));
        }
    }
}
