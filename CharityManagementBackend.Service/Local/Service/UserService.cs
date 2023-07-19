using CharityManagementBackend.Core.Helpers;
using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Service.Local.Interface;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CharityManagementBackend.Service.Local.Service
{
    public class UserService : IUserService
    {
        IRepositoryWrapper _repository;
        private readonly AppSettings _appSettings;

        public UserService(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
        }

        public Tuple<bool, bool> CheckUserNameAndEmailExist(string email, string username)
        {
            bool emailValid = _repository.User.IsExistEmail(email.FixText());
            bool userNameValid = _repository.User.IsExistUserName(username.FixText());

            return Tuple.Create(emailValid, userNameValid);
        }
        public int AddUser(User user)
        {
            _repository.User.AddUser(user);
            return user.UserId;
        }

        public int GetAllUsers()
        {
            return _repository.User.GetAllUsers();
        }

        public User GetUserById(int userId)
        {
            return _repository.User.GetUserById(userId);
        }
        public void EditUser(User user)
        {
            _repository.User.EditUser(user);
        }
        public bool ExistUserByRoleId(int RoleId)
        {
            return _repository.User.ExistUserByRoleId(RoleId);
        }
        public User LoginUser(string username, string password)
        {
            return _repository.User.GetUserLogin(username.FixText(), password);
        }
        public Token GenToken(User user)
        {
            return new Token(GenerateToken(user));
        }
        private string GenerateToken(User user, int? tokenValidateInMinutes = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.TokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.DisplayName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenValidateInMinutes ?? _appSettings.TokenValidateInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public List<User> GetAllUsersExceptCurrent(int userId)
        {
            return _repository.User.GetAllUsersExceptCurrent(userId);
        }

        public User GetUserByUsername(string username)
        {
            return _repository.User.GetUserByUsername(username);
        }

        public List<User> GetAllUsers(int pageSize, int pageNumber)
        {
            return _repository.User.GetAllUsers(pageSize, pageNumber);
        }
    }

}
