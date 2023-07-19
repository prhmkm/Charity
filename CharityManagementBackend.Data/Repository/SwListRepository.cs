using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Data.Interface;
using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Data.Repository
{
    public class SwListRepository : BaseRepository<SwList>, ISwListRepository
    {
        charityContext _repositoryContext;
        public SwListRepository(charityContext RepositoryContext) : base(RepositoryContext)
        {
            _repositoryContext = RepositoryContext;
        }

        public List<SwList> GetAll()
        {
            return FindAll().ToList();
        }

        public SwList GetById(int id)
        {
            return FindByCondition(o => o.Id == id).FirstOrDefault();
        }
    }
}
