using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Data.Interface;
using CharityManagementBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CharityManagementBackend.Data.Repository
{
    public class ProcessingRepository : BaseRepository<CharityTran>, IProcessingRepository
    {
        charityContext _repositoryContext;
        public ProcessingRepository(charityContext RepositoryContext) : base(RepositoryContext)
        {
            _repositoryContext = RepositoryContext;
        }

        public void DeleteCharityTrans(string TranDate)
        {
            try
            {
                _repositoryContext.Database.SetCommandTimeout(1800000);
                _repositoryContext.Database.ExecuteSqlRaw("STP_ClearCharityTran '" + TranDate + "'");
            }
            catch (Exception ex)
            {

            }
        }

        public CharityTran GetCharityTranByDate(string TranDate)
        {
            return FindByCondition(w => w.TRANDATE == (TranDate.Replace("/", ""))).FirstOrDefault();
        }

        public List<CharityTran> Procesing()
        {
            return FindAll().OrderBy(o => o.TRANDATE).ToList();
        }

        public void ProcesingCharityTransList(string TranDate1, string TranDate2)
        {
            try
            {
                _repositoryContext.Database.SetCommandTimeout(1800000);
                _repositoryContext.Database.ExecuteSqlRaw("STP_LoadCharityTrans '" + TranDate1 + "','" + TranDate2 + "'");
            }
            catch (Exception ex)
            {

            }
        }

        public void ProcessCharityTrans(string TranDate)
        {
            try
            {
                _repositoryContext.Database.SetCommandTimeout(1800000);
                _repositoryContext.Database.ExecuteSqlRaw("STP_InsertPaymentes '" + TranDate + "'");
                _repositoryContext.Database.ExecuteSqlRaw("STP_BackupCharityTrans '" + TranDate + "'");
                _repositoryContext.Database.ExecuteSqlRaw("STP_ClearCharityTran '" + TranDate + "'");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
