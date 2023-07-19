using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Service.Local.Interface;
using CharityManagementBackend.Service.Local.Service;
using Microsoft.Extensions.Options;


namespace CharityManagementBackend.Service.Base
{
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly IOptions<AppSettings> _appSettings;
        private IRepositoryWrapper _repository;
        public ServiceWrapper(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            _repository = repository;
        }
        public IUserService User => new UserService(_repository, _appSettings);
        public IPaymentServices Payment => new PaymentServices(_repository, _appSettings);

        public IProcessingServices Processing => new ProcessingServices(_repository, _appSettings);

        public ICharityServices Charity => new CharityServices(_repository, _appSettings);

        public IReportsServices Reports => new ReportsServices(_repository, _appSettings);

        public ISwListService SwList => new SwListService(_repository);

        public void Save()
        {
            _repository.Save();
        }
    }
}
