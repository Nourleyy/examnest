namespace ExamNest.Interfaces
{
    public interface IGeneric<U> where U : class
    {


        Task<bool> Delete(int id);
        Task<U?> Update(int id, U entity);
        Task<U?> Create(U examDto);


    }
}
