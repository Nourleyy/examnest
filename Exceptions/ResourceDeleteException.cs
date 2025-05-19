namespace ExamNest.Errors;

public class ResourceDeleteException(string msg) : ApplicationException(msg);