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







}
