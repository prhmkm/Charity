using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Domain.DTOs;
using CharityManagementBackend.Service.Local.Interface;
using Microsoft.Extensions.Options;

namespace CharityManagementBackend.Service.Local.Service
{
    public class ReportsServices : IReportsServices
    {
        IRepositoryWrapper _repository;
        private readonly AppSettings _appSettings;
        public ReportsServices(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
        }

        public List<BackupCharityTranDTO> Transaction(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, string? TranPan)
        {
            return _repository.Reports.Transaction(date1, date2, serviceID, SwCode, TranPan);
        }

        public List<BackupCharityTranDTO> Transaction(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, string? TranPan, int pageSize, int pageNumber)
        {
            return _repository.Reports.Transaction(date1, date2, serviceID, SwCode, TranPan, pageSize, pageNumber);
        }
    }
}
