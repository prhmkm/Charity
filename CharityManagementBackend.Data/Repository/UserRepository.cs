using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Data.Interface;
using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Data.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        charityContext _repositoryContext;
        public UserRepository(charityContext repositoryContext) : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void AddUser(User user)
        {
            Create(user);
            Save();
        }

        public void EditUser(User user)
        {
            Update(user);
            Save();
        }

        public bool ExistUserByRoleId(int RoleId)
        {
            return FindByCondition(w => w.RoleId == RoleId && w.IsDeleted == false).Any();
        }

        public int GetAllUsers()
        {
            return FindByCondition(w => w.IsDeleted == false).Count();
        }
        public List<User> GetAllUsers( int pageSize,int pageNumber)
        {
            return FindByCondition(w => w.IsDeleted == false).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        public List<User> GetAllUsersExceptCurrent(int userId)
        {
            return FindByCondition(w =>  w.UserId != userId && w.IsDeleted == false).ToList();
        }

        public User GetUserById(int userId)
        {
            return FindByCondition(w => w.UserId == userId && w.IsDeleted == false).FirstOrDefault();
        }

        public User GetUserByUsername(string username)
        {
            var x = _repositoryContext.Users.Where(w => w.UserName == username && w.IsDeleted == false);

            return x.FirstOrDefault();
        }

        public User GetUserLogin(string username, string password)
        {
            return FindByCondition(w => w.UserName == username && w.PassWord == password && w.IsDeleted == false).FirstOrDefault();
        }

        public bool IsExistEmail(string email)
        {
            return FindByCondition(w => w.Email == email && w.IsDeleted == false).Any();
        }

        public bool IsExistUserName(string username)
        {
            return FindByCondition(w => w.UserName == username && w.IsDeleted == false).Any();
        }
    }

}
