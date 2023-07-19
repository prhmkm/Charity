using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Data.Interface
{
    public interface IUserRepository
    {
        int GetAllUsers();
        List<User> GetAllUsers(int pageSize,  int pageNumber);
        User GetUserById(int userId);
        void EditUser(User user);
        bool ExistUserByRoleId(int RoleId);
        void AddUser(User user);
        List<User> GetAllUsersExceptCurrent(int userId);
        bool IsExistEmail(string email);
        bool IsExistUserName(string username);
        User GetUserLogin(string username, string password);
        User GetUserByUsername(string username);
    }
}
