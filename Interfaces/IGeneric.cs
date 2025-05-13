namespace ExamNest.Interfaces
{
    public interface IGeneric<U> where U : class
    {


        Task<bool> Delete(int id);
        Task<int?> Update(int id, U entity);
        Task<decimal?> Create(U entityDto);


    }
}
