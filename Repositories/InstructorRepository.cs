using AutoMapper;
using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Repositories
{


    public class InstructorRepository : GenericRepository, IInstructorRepository
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IMapper mapper;
        public InstructorRepository(AppDBContext appDB, ITrackRepository trackRepository, IBranchRepository branchRepository, IMapper _mapper) : base(appDB)
        {
            _trackRepository = trackRepository;
            _branchRepository = branchRepository;
            mapper = _mapper;
        }

        public async Task<UserDTO?> Create(UserDTO examDto)
        {
            var trackSearch = await _trackRepository.GetById(examDto.TrackId);
            var branchSearch = await _branchRepository.GetById(examDto.BranchId);

            if (trackSearch == null || branchSearch == null) return null;

            //var UserSearch = _context.Users.FirstOrDefault(u => u.Id == instructor.UserId);
            //if (UserSearch == null)
            //{
            //    return BadRequest("User Id not found");
            //}
            var result = await appDBContextProcedures.CreateInstructorAsync(examDto.BranchId, examDto.TrackId, examDto.UserId);
            return result.Count > 0 ? examDto : null;

        }



        public async Task<IEnumerable<UserViewDTO>> GetAll(int page)
        {
            var instructors = await _appDBContext.Instructors
              .Include(t => t.Track)
              .Include(b => b.Branch)
              .Include(u => u.User)
              .Skip(CalculatePagination(page))
                .Take(LimitPerPage)
              .ToListAsync();

            return mapper.Map<IEnumerable<UserViewDTO>>(instructors);

        }

        public async Task<GetInstructorByIDResult?> GetById(int id)
        {
            var instructor = await appDBContextProcedures.GetInstructorByIDAsync(id);

            return instructor.FirstOrDefault();
        }

        public async Task<UserDTO?> Update(int id, UserDTO entity)
        {
            var instructor = await GetById(id);
            if (instructor == null ||
                await _trackRepository.GetById(entity.TrackId) is null ||
                await _branchRepository.GetById(entity.BranchId) is null)
            {
                return null;
            }

            //var UserSearch = _context.Users.FirstOrDefault(u => u.Id == instructor.UserId);
            //if (UserSearch == null)
            //{
            //    return BadRequest("User Id not found");
            //}

            var result = await appDBContextProcedures.UpdateInstructorAsync(id, entity.BranchId, entity.TrackId, entity.UserId);
            return result.Count > 0 ? entity : null;


        }
        public async Task<bool> Delete(int id)
        {
            var deleted = await appDBContextProcedures.DeleteInstructorAsync(id);
            return deleted.Count > 0;
        }
    }




}
