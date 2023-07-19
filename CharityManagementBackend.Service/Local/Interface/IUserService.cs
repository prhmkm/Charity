using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Service.Local.Interface
{
    public interface IUserService
    {
        int GetAllUsers();
        List<User> GetAllUsers(int pageSize, int pageNumber);
        User GetUserById(int userId);
        void EditUser(User user);
        bool ExistUserByRoleId(int RoleId);
        Tuple<bool, bool> CheckUserNameAndEmailExist(string email, string username);
        int AddUser(User user);
        User LoginUser(string username, string password);
        Token GenToken(User user);
        List<User> GetAllUsersExceptCurrent(int userId);
        User GetUserByUsername(string username);
    }
}
