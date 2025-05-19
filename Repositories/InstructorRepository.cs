using AutoMapper;
using ExamNest.DTO.Authentication;
using ExamNest.Errors;
using ExamNest.Interfaces;
using ExamNest.Models;
using ExamNest.Services;
using Microsoft.EntityFrameworkCore;

namespace ExamNest.Repositories
{


    public class InstructorRepository : GenericRepository, IInstructorRepository
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IUserManagement userManagementService;
        private readonly IMapper mapper;
        public InstructorRepository(AppDBContext appDB, ITrackRepository trackRepository, IBranchRepository branchRepository, IMapper _mapper, IUserManagement userManagementService) : base(appDB)
        {
            _trackRepository = trackRepository;
            _branchRepository = branchRepository;
            mapper = _mapper;
            this.userManagementService = userManagementService;
        }

        public async Task<decimal?> Create(Instructor userDto)
        {
            var trackSearch = await _trackRepository.GetById(userDto.TrackId);
            var branchSearch = await _branchRepository.GetById(userDto.BranchId);

            if (trackSearch == null || branchSearch == null) throw new ResourceNotFoundException("Either Track or Branch Not Found");

            var userSearch = userManagementService.IsUserExistById(userDto.UserId);
            if (userSearch == null)
            {
                throw new ResourceNotFoundException("User Id not found");
            }

            var result = await AppDbContextProcedures.CreateInstructorAsync(userDto.BranchId, userDto.TrackId, userDto.UserId);
            return result.FirstOrDefault()?.InstructorID;

        }



        public async Task<IEnumerable<InstructorViewDto>> GetAll(int page)
        {
            var instructors = await AppDbContext.Instructors
              .Include(t => t.Track)
              .Include(b => b.Branch)
              .Include(u => u.User)
              .Skip(CalculatePagination(page))
                .Take(LimitPerPage)
              .ToListAsync();

            return mapper.Map<IEnumerable<InstructorViewDto>>(instructors);

        }

        public async Task<GetInstructorByIDResult?> GetById(int id)
        {
            var instructor = await AppDbContextProcedures.GetInstructorByIDAsync(id);

            return instructor.FirstOrDefault();
        }

        public async Task<int?> Update(int id, Instructor entity)
        {
            var instructor = await GetById(id);
            if (instructor == null ||
                await _trackRepository.GetById(entity.TrackId) is null ||
                await _branchRepository.GetById(entity.BranchId) is null)
            {
                throw new ResourceNotFoundException("Either Track or Branch or instructor Not Found");
            }

            var userSearch = userManagementService.IsUserExistById(entity.UserId);
            if (userSearch == null)
            {
                throw new ResourceNotFoundException("User Id not found");
            }


            var result = await AppDbContextProcedures.UpdateInstructorAsync(id, entity.BranchId, entity.TrackId, entity.UserId);
            return result.FirstOrDefault()?.RowsUpdated > 0 ? id : null;


        }
        public async Task<bool> Delete(int id)
        {
            var instructor = await GetById(id);
            if (instructor == null)
            {
                throw new ResourceNotFoundException("Instructor not found to be deleted");
            }
            var deleted = await AppDbContextProcedures.DeleteInstructorAsync(id);

            return deleted.FirstOrDefault()?.RowsDeleted > 0;
        }

        public async Task<InstructorViewDto?> GetInstructorByUserId(string userId)
        {
            var instructor = await AppDbContext.Instructors
                .FirstOrDefaultAsync(i => i.UserId == userId);

            return instructor == null ? null : mapper.Map<InstructorViewDto>(instructor);
        }
    }




}
