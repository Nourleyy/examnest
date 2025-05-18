using ExamNest.Filters;
using Microsoft.AspNetCore.Identity;

namespace ExamNest.Errors
{

    public class ResourceDeleteException : ApplicationException
    {



        public ResourceDeleteException(string msg)
            : base(msg) { }


    }

    public class ResourceNotFoundException : ApplicationException
    {
        public ResourceNotFoundException(string msg)
            : base(msg) { }
    }

    public class ResourceAlreadyExistsException : ApplicationException
    {
        public ResourceAlreadyExistsException(string msg)
            : base(msg) { }
    }


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
