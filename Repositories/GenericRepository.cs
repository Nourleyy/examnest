using ExamNest.Models;

namespace ExamNest.Repositories
{

    public abstract class GenericRepository
    {
        protected const int LimitPerPage = 10;

        protected readonly AppDBContext _appDBContext;
        protected readonly IAppDBContextProcedures appDBContextProcedures;
        public GenericRepository(AppDBContext appDB)
        {

            _appDBContext = appDB;
            appDBContextProcedures = _appDBContext.GetProcedures();

        }


        protected int CalculatePagination(int skip = 0, int page = 1)
        {
            return (page - 1) * LimitPerPage;

        }




    }
}
