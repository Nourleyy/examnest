using AutoMapper;
using ExamNest.DTO;
using ExamNest.Errors;
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

        public async Task<decimal?> Create(UserDTO userDto)
        {
            var trackSearch = await _trackRepository.GetById(userDto.TrackId);
            var branchSearch = await _branchRepository.GetById(userDto.BranchId);

            if (trackSearch == null || branchSearch == null) throw new ResourceNotFoundException("Either Track or Branch Not Found");

            //var UserSearch = _context.Users.FirstOrDefault(u => u.Id == instructor.UserId);
            //if (UserSearch == null)
            //{
            //    return BadRequest("User Id not found");
            //}
            var result = await appDBContextProcedures.CreateInstructorAsync(userDto.BranchId, userDto.TrackId, userDto.UserId);
            return result.FirstOrDefault()?.InstructorID;

        }



        public async Task<IEnumerable<UserViewDTO>> GetAll(int page)
        {
            var instructors = await _appDBContext.Instructors
              .Include(t => t.Track)
              .Include(b => b.Branch)
              //.Include(u => u.User)
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

        public async Task<int?> Update(int id, UserDTO entity)
        {
            var instructor = await GetById(id);
            if (instructor == null ||
                await _trackRepository.GetById(entity.TrackId) is null ||
                await _branchRepository.GetById(entity.BranchId) is null)
            {
                throw new ResourceNotFoundException("Either Track or Branch or instructor Not Found");
            }

            //var UserSearch = _context.Users.FirstOrDefault(u => u.Id == instructor.UserId);
            //if (UserSearch == null)
            //{
            //    return BadRequest("User Id not found");
            //}

            var result = await appDBContextProcedures.UpdateInstructorAsync(id, entity.BranchId, entity.TrackId, entity.UserId);
            return result.FirstOrDefault()?.RowsUpdated > 0 ? id : null;


        }
        public async Task<bool> Delete(int id)
        {
            var instructor = await GetById(id);
            if (instructor == null)
            {
                throw new ResourceNotFoundException("Instructor not found to be deleted");
            }
            var deleted = await appDBContextProcedures.DeleteInstructorAsync(id);

            return deleted.FirstOrDefault()?.RowsDeleted > 0;
        }
    }




}
