using Microsoft.Extensions.Options;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Data.Interface;
using CharityManagementBackend.Data.Repository;
using CharityManagementBackend.Domain.DTOs;

namespace CharityManagementBackend.Data.Base
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private charityContext _repoContext;
        protected SampleContext SampleContext { get; set; }

        public RepositoryWrapper(charityContext repositoryContext, SampleContext _SampleContext)
        {
            _repoContext = repositoryContext;
            SampleContext = _SampleContext;
        }
        public IUserRepository User => new UserRepository(_repoContext);

        public IPaymentRepository Payment => new PaymentRepository(_repoContext,SampleContext);

        public IProcessingRepository Processing => new ProcessingRepository(_repoContext);

        public ICharityRepository Charity => new CharityRepository(_repoContext);

        public IReportsRepository Reports => new ReportsRepository(_repoContext);

        public ISwListRepository SwList => new SwListRepository(_repoContext);

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
