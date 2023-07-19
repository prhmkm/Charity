using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Service.Local.Interface;
using Microsoft.Extensions.Options;

namespace CharityManagementBackend.Service.Local.Service
{
    public class ProcessingServices : IProcessingServices
    {
        IRepositoryWrapper _repository;
        private readonly AppSettings _appSettings;
        public ProcessingServices(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
        }

        public List<CharityTran> Procesing()
        {
            return _repository.Processing.Procesing();
        }

        public void ProcessCharityTrans(string TranDate)
        {
            _repository.Processing.ProcessCharityTrans(TranDate);
        }


        public void DeleteCharityTrans(string TranDate)
        {
            _repository.Processing.DeleteCharityTrans(TranDate);
        }

        public CharityTran GetCharityTranByDate(string TranDate)
        {
            return _repository.Processing.GetCharityTranByDate(TranDate);
        }

        public void ProcesingCharityTransList(string TranDate1, string TranDate2)
        {
            _repository.Processing.ProcesingCharityTransList(TranDate1, TranDate2);
        }
    }
}
