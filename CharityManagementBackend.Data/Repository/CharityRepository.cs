using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Data.Interface;
using CharityManagementBackend.Domain.Models;
using static CharityManagementBackend.Domain.DTOs.CharityDTO;

namespace CharityManagementBackend.Data.Repository
{
    public class CharityRepository : BaseRepository<CharityService>, ICharityRepository
    {
        charityContext _repositoryContext;
        public CharityRepository(charityContext RepositoryContext) : base(RepositoryContext)
        {
            _repositoryContext = RepositoryContext;
        }

        public void AddCharity(CharityService charity)
        {
            Create(charity);
            Save();
        }

        public int CharityList()
        {
            return _repositoryContext.CharityServices.Where(w => w.IsDelete == false).Count();
        }
        public List<CharityListResponse> CharityList(int pageSize,  int pageNumber)
        {
            return _repositoryContext.CharityServices.Where(w => w.IsDelete == false).Select(s => new CharityListResponse { ServiceID = s.TSRVCID, ServiceName = s.ServiceName, Account = s.Account, Iban = s.Iban, ContactName = s.ContactName, ContactNumber = s.ContactNumber, IsActive = s.IsActive, SwCode = s.SwCode, SwTitle = _repositoryContext.SwLists.FirstOrDefault(o => o.Id == s.SwCode).SwTitle, IsDelete = s.IsDelete }).OrderBy(o => o.SwCode).ThenBy(o => o.ServiceID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<CharityListResponse> CharityListBySwCode(List<int>? swCode)
        {
            List<string> li = new List<string>();
            foreach (var item in swCode)
            {
                li.Add(item.ToString());
            }
            return _repositoryContext.CharityServices.Where(s => (s.IsDelete == false) &&
            (s.IsActive == true) &&
            (swCode.Count() != 0 ? li.Contains(s.SwCode.ToString()) : true))
                .Select(s=>new CharityListResponse { ServiceID = s.TSRVCID, ServiceName = s.SwCode + "- " + s.ServiceName, Account = s.Account, Iban = s.Iban, ContactName = s.ContactName, ContactNumber = s.ContactNumber, IsActive = s.IsActive, SwCode = s.SwCode , SwTitle = _repositoryContext.SwLists.FirstOrDefault(o => o.Id == s.SwCode).SwTitle, IsDelete=s.IsDelete })
        .ToList();
        }

        public bool ExistByKey(int serviceId, int swCode)
        {
            
            int count = _repositoryContext.CharityServices.Where(s=>s.TSRVCID == serviceId && s.SwCode == swCode).Count();
            if(count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public CharityService GetCharityById(int serviceID, int swCode)
        {
            return FindByCondition(w => w.TSRVCID == serviceID && w.SwCode == swCode).FirstOrDefault();
        }

        public bool IsExistServiceName(int serviceID)
        {
            return FindByCondition(w => w.TSRVCID == serviceID && w.IsDelete == false).Any();
        }

        public void UpdateCharity(CharityService charityService)
        {
            Update(charityService);
            Save();
        }
    }
}
