using Microsoft.AspNetCore.Identity;

namespace ExamNest.Errors
{
    public class UserCreationErrorException : ApplicationException
    {
        public UserCreationErrorException(IEnumerable<IdentityError> identityErrors)
        {

            var errors = identityErrors.Select(e => e.Description).ToList();
            var errorMessage = string.Join(",", errors);

            throw new BadHttpRequestException(errorMessage);


        }

        public UserCreationErrorException(string msg)
            : base(msg) { }
    }




}
