using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Service.Local.Interface;
using Microsoft.Extensions.Options;
using static CharityManagementBackend.Domain.DTOs.CharityDTO;

namespace CharityManagementBackend.Service.Local.Service
{
    public class CharityServices : ICharityServices
    {
        IRepositoryWrapper _repository;
        private readonly AppSettings _appSettings;

        public CharityServices(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
        }
        public void AddCharity(CharityService charity)
        {
            _repository.Charity.AddCharity(charity);
        }

        public bool CheckServiceNameExist(int serviceID)
        {
            return _repository.Charity.IsExistServiceName(serviceID);
        }

        public int CharityList()
        {
            return _repository.Charity.CharityList();
        }

        public CharityService GetCharityById(int serviceID, int swCode)
        {
            return _repository.Charity.GetCharityById(serviceID , swCode);
        }

        public void UpdateCharity(CharityService charityService)
        {
            _repository.Charity.UpdateCharity(charityService);
        }

        public List<CharityListResponse> CharityListBySwCode(List<int>? swCode)
        {
            return _repository.Charity.CharityListBySwCode(swCode); 
        }

        public bool ExistByKey(int serviceId, int swCode)
        {
            return _repository.Charity.ExistByKey(serviceId, swCode);
        }

        public List<CharityListResponse> CharityList(int pageSize, int pageNumber)
        {
            return _repository.Charity.CharityList(pageSize, pageNumber);
        }
    }
}
