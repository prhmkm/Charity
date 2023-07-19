using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Data.Interface
{
    public interface ISwListRepository
    {
        List<SwList> GetAll();
        SwList GetById(int id);
    }
}
