using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Service.Local.Interface
{
    public interface ISwListService
    {
        List<SwList> GetAll();
        SwList GetById(int id);
    }
}
