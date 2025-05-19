using ExamNest.DTO.Authentication;
using ExamNest.Models;

namespace ExamNest.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<CurrentUserDto?> GetCurrentUser();
    }
    public class AuthenticationRepository : GenericRepository, IAuthenticationRepository
    {
        public AuthenticationRepository(AppDBContext appDb) : base(appDb)
        {
        }

        public async Task<CurrentUserDto?> GetCurrentUser()
        {
            return await GetCurrentAsync();
        }
    }
}
