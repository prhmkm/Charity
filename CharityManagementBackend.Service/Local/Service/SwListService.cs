using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Service.Local.Interface;

namespace CharityManagementBackend.Service.Local.Service
{
    public class SwListService : ISwListService
    {
        IRepositoryWrapper _repository;

        public SwListService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public List<SwList> GetAll()
        {
            return _repository.SwList.GetAll();
        }

        public SwList GetById(int id)
        {
            return _repository.SwList.GetById(id);
        }
    }
}
