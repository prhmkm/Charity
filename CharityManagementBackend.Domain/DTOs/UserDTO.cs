namespace CharityManagementBackend.Domain.DTOs
{
    public class UserDTO
    {
        public class UserResponse
        {
            public string UserName { get; set; }
            public int UserId { get; set; }
            public int RoleId { get; set; }
            public string Address { get; set; }
            public string BirthDate { get; set; }
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string Image { get; set; }
            public string ImageThumb { get; set; }
            public string LastName { get; set; }
            public string Mobile { get; set; }
            public string NationalCode { get; set; }
            public string Phone { get; set; }
            public string RoleTitle { get; set; }
        }
        public class AddUserRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public string PassWord { get; set; }
            public int RoleId { get; set; }
            public string NationalCode { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Mobile { get; set; }
            public string BirthDate { get; set; }
            public string Image { get; set; }
        }
        public class EditUserRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string UserName { get; set; }
            public int RoleId { get; set; }
            public string NationalCode { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public string Mobile { get; set; }
            public string BirthDate { get; set; }
            public int UserId { get; set; }
            public bool? IsActive { get; set; }
        }
        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }
        public class LoginResponse
        {
            public string BirthDate { get; set; }
            public string DisplayName { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string Image { get; set; }
            public string ImageThumb { get; set; }
            public string LastName { get; set; }
            public string Mobile { get; set; }
            public string NationalCode { get; set; }
            public string Phone { get; set; }
            public int RoleId { get; set; }
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public string UserName { get; set; }
        }
        public class RefreshTokenRequest
        {
            public string Username { get; set; }
            public string RefreshToken { get; set; }
        }
    }
}
