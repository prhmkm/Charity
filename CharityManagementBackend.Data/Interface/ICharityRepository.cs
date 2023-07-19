using CharityManagementBackend.Domain.Models;
using static CharityManagementBackend.Domain.DTOs.CharityDTO;

namespace CharityManagementBackend.Data.Interface
{
    public interface ICharityRepository
    {
        void AddCharity(CharityService charity);
        bool IsExistServiceName(int serviceID);
        List<CharityListResponse> CharityList( int pageSize,  int pageNumber);
        int CharityList();
        List<CharityListResponse> CharityListBySwCode(List<int>? swCode);
        CharityService GetCharityById(int serviceID, int swCode);
        bool ExistByKey(int serviceId, int swCode);
        void UpdateCharity(CharityService charityService);
    }
}
