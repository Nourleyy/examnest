namespace ExamNest.Errors;

public class ResourceNotFoundException(string msg) : ApplicationException(msg);