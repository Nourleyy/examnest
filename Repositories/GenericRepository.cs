using ExamNest.Models;

namespace ExamNest.Repositories
{

    public abstract partial class GenericRepository
    {
        protected readonly AppDBContext _appDBContext;
        protected readonly IAppDBContextProcedures appDBContextProcedures;
        public GenericRepository(AppDBContext appDB)
        {

            _appDBContext = appDB;
            appDBContextProcedures = _appDBContext.GetProcedures();

        }




    }
}
