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
        public async Task<bool> Create(ChoiceDTO choice)
        {
            var ListCreated = await appDBContextProcedures.CreateChoiceAsync(choice.QuestionId, choice.ChoiceLetter, choice.ChoiceText);

            return ListCreated.Count > 0;
        }

        public async Task<bool> Update(ChoiceDTO choice, int id)
        {
            var ListUpdated = await appDBContextProcedures.UpdateChoiceAsync(id, choice.ChoiceLetter, choice.ChoiceText);
            return ListUpdated[0].RowsUpdated > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var ListDeleted = await appDBContextProcedures.DeleteChoiceAsync(id);
            return ListDeleted[0].RowsDeleted > 0;
        }

    }


}
