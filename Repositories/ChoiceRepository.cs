using ExamNest.DTO;
using ExamNest.Interfaces;
using ExamNest.Models;

namespace ExamNest.Repositories
{
    public class ChoiceRepository : GenericRepository, IChoiceRepository
    {


        public ChoiceRepository(AppDBContext appDB) : base(appDB)
        {
        }
        public async Task<GetChoiceByIDResult?> GetById(int id)
        {
            var ChoiceList = await appDBContextProcedures.GetChoiceByIDAsync(id);

            return ChoiceList.FirstOrDefault();
        }
        public async Task<ChoiceDTO?> Create(ChoiceDTO choice)
        {
            var ListCreated = await appDBContextProcedures.CreateChoiceAsync(choice.QuestionId, choice.ChoiceLetter, choice.ChoiceText);

            return ListCreated.Count > 0 ? choice : null;
        }

        public async Task<ChoiceDTO?> Update(int id, ChoiceDTO choice)
        {
            var ListUpdated = await appDBContextProcedures.UpdateChoiceAsync(id, choice.ChoiceLetter, choice.ChoiceText);
            return ListUpdated.Count > 0 ? choice : null;
        }

        public async Task<bool> Delete(int id)
        {
            var ListDeleted = await appDBContextProcedures.DeleteChoiceAsync(id);
            return ListDeleted[0].RowsDeleted > 0;
        }

    }


}
