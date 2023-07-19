using CharityManagementBackend.Core.Helpers;
using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;
using static CharityManagementBackend.Domain.DTOs.UserDTO;

namespace PrivatizationBackend.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        IServiceWrapper _service;
        private readonly AppSettings _appSettings;

        public UserController(IServiceWrapper service, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }

        public class ChangePass
        {
            public int userId { get; set; }
            public string oldPassword { get; set; }
            public string newPassword { get; set; }
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] AddUserRequest user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "خطای ناشناخته", Value = new { }, Error = new { ErrorMsg = ModelState } });
                }
                if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName) && !string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.PassWord) && !string.IsNullOrEmpty(user.NationalCode) && !string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Phone) && !string.IsNullOrEmpty(user.Mobile))
                {
                    if (user.RoleId > 0)
                    {
                        Tuple<bool, bool> validate = _service.User.CheckUserNameAndEmailExist(user.Email, user.UserName);

                        if (validate.Item1)
                        {
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "ایمیل تکراری است", Value = new { }, Error = new { } });

                        }
                        else if (validate.Item2)
                        {
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "نام کاربری تکراری است", Value = new { }, Error = new { } });

                        }
                        if (user.NationalCode.Length != 10)
                        {
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "کد ملی نامعتبر است", Value = new { }, Error = new { } });

                        }
                        User userCreated = new User()
                        {
                            FirstName = user.FirstName.FixText(),
                            LastName = user.LastName.FixText(),
                            UserName = user.UserName.FixText(),
                            DisplayName = user.FirstName.FixText() + " " + user.LastName.FixText(),
                            PassWord = user.PassWord,
                            RoleId = user.RoleId,
                            NationalCode = user.NationalCode.FixText(),
                            Email = user.Email.FixText(),
                            Address = user.Address,
                            Phone = user.Phone.FixText(),
                            Mobile = user.Mobile.FixText(),
                            BirthDate = user.BirthDate,
                            Image = user.Image,
                            ImageThumb = user.Image,
                            IsActive = true
                        };
                        _service.User.AddUser(userCreated);
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "کاربر با موفقیت ثبت شد", Value = new { }, Error = new { } });
                    }
                    else
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "سطح کاربری انتخاب نشده است", Value = new { }, Error = new { } });
                    }
                }
                else
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "یک یا چند مورد اجباری فرستاده نشده است", Value = new { }, Error = new { } });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }

        }


        [HttpPost("EditUser")]
        public IActionResult EditUser([FromBody] EditUserRequest Editeduser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "خطای ناشناخته", Value = new { }, Error = new { ErrorMsg = ModelState } });
                }
                if (!string.IsNullOrEmpty(Editeduser.FirstName) && !string.IsNullOrEmpty(Editeduser.LastName) && !string.IsNullOrEmpty(Editeduser.UserName) && !string.IsNullOrEmpty(Editeduser.NationalCode) && !string.IsNullOrEmpty(Editeduser.Email) && !string.IsNullOrEmpty(Editeduser.Phone) && !string.IsNullOrEmpty(Editeduser.Mobile))
                {
                    if (Editeduser.RoleId > 0)
                    {
                        User user = _service.User.GetUserById(Editeduser.UserId);
                        if (user != null)
                        {
                            if (user.Email != Editeduser.Email && user.UserName != Editeduser.UserName)
                            {
                                Tuple<bool, bool> validate = _service.User.CheckUserNameAndEmailExist(Editeduser.Email, Editeduser.UserName);
                                if (validate.Item1)
                                {
                                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "ایمیل تکراری است", Value = new { }, Error = new { } });

                                }
                                else if (validate.Item2)
                                {
                                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "نام کاربری تکراری است", Value = new { }, Error = new { } });

                                }
                            }
                            if (user.Email != Editeduser.Email)
                            {
                                Tuple<bool, bool> validate = _service.User.CheckUserNameAndEmailExist(Editeduser.Email, "");
                                if (validate.Item1)
                                {
                                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "ایمیل تکراری است", Value = new { }, Error = new { } });

                                }
                            }
                            if (user.UserName != Editeduser.UserName)
                            {
                                Tuple<bool, bool> validate = _service.User.CheckUserNameAndEmailExist("", Editeduser.UserName);
                                if (validate.Item2)
                                {
                                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "نام کاربری تکراری است", Value = new { }, Error = new { } });
                                }
                            }
                            user.FirstName = Editeduser.FirstName.FixText();
                            user.LastName = Editeduser.LastName.FixText();
                            user.UserName = Editeduser.UserName.FixText();
                            user.DisplayName = Editeduser.FirstName.FixText() + " " + Editeduser.LastName.FixText();
                            user.RoleId = Editeduser.RoleId;
                            user.NationalCode = Editeduser.NationalCode.FixText();
                            user.Email = Editeduser.Email.FixText();
                            user.Address = Editeduser.Address;
                            user.Phone = Editeduser.Phone.FixText();
                            user.Mobile = Editeduser.Mobile.FixText();
                            user.BirthDate = Editeduser.BirthDate;
                            user.IsActive = Editeduser.IsActive;
                            _service.User.EditUser(user);
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "کاربر با موفقیت ویرایش شد", Value = new { }, Error = new { } });
                        }
                        else
                        {
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "کاربر مورد نظر یافت نشد", Value = new { }, Error = new { } });
                        }
                    }
                    else
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "شناسه کاربر نامعتبر است", Value = new { }, Error = new { } });

                    }
                }
                else
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "اطلاعات وارد شده نامعتبر است", Value = new { }, Error = new { } });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser([FromHeader] int userId)
        {
            try
            {
                if (userId > 0)
                {
                    User chek = _service.User.GetUserById(userId);
                    if (chek != null)
                    {
                        chek.IsDeleted = true;
                        _service.User.EditUser(chek);

                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "کاربر با موفقیت حذف شد", Value = new { }, Error = new { } });
                    }
                    else
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "کاربر مورد نظر یافت نشد", Value = new { }, Error = new { } });
                    }
                }
                else
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "شناسه کاربر نامعتبر است", Value = new { }, Error = new { } });

                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePass changePass)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "خطای ناشناخته", Value = new { }, Error = new { ErrorMsg = ModelState } });
                }
                if (changePass.userId == 0)
                {
                    if (!string.IsNullOrEmpty(changePass.oldPassword) && !string.IsNullOrEmpty(changePass.newPassword))
                    {
                        var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                        User user = _service.User.GetUserById(Convert.ToInt32(userId));
                        if (user.IsActive == true)
                        {
                            if (user.PassWord == changePass.oldPassword)
                            {
                                user.PassWord = changePass.newPassword;
                                _service.User.EditUser(user);

                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "تغییر کلمه عبور با موفقیت انجام شد", Value = new { }, Error = new { } });
                            }
                            else
                            {
                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.NotFound, Message = "کلمه عبور قدیمی وارد شده اشتباه است", Value = new { }, Error = new { } });
                            }
                        }
                        else
                        {
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "کاربر غیرفعال است", Value = new { }, Error = new { } });
                        }
                    }
                    else
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "اطلاعات مورد نیاز وارد نشده است", Value = new { }, Error = new { } });
                    }
                }
                if (!string.IsNullOrEmpty(changePass.newPassword))
                {
                    User user = _service.User.GetUserById(changePass.userId);
                    if (user == null)
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.NotFound, Message = "شناسه کاربر معتبر نمی باشد", Value = new { }, Error = new { } });
                    }
                    if (user.IsActive == false)
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "کاربر غیرفعال است", Value = new { }, Error = new { } });
                    }
                    user.PassWord = changePass.newPassword;
                    _service.User.EditUser(user);

                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "تغییر کلمه عبور با موفقیت انجام شد", Value = new { }, Error = new { } });
                }
                else
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "کلمه عبور وارد نشده است", Value = new { }, Error = new { } });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

        [HttpGet("UsersList")]
        public IActionResult UsersList([FromHeader] int pageSize, [FromHeader] int pageNumber)
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                List<User> users = _service.User.GetAllUsers(pageSize, pageNumber);
                List<UserResponse> UserList = users.Select(s => new UserResponse
                {
                    UserId = s.UserId,
                    UserName = s.UserName,
                    RoleId = s.RoleId,
                    Address = s.Address,
                    BirthDate = s.BirthDate,
                    DisplayName = s.DisplayName,
                    Email = s.Email,
                    FirstName = s.FirstName,
                    Image = s.Image,
                    ImageThumb = s.ImageThumb,
                    LastName = s.LastName,
                    Mobile = s.Mobile,
                    NationalCode = s.NationalCode,
                    Phone = s.Phone
                }).OrderBy(o => o.UserId).ToList();
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "لیست کاربران با موفقیت ارسال شد", Value = new { response = UserList , max = _service.User.GetAllUsers() }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }


        [HttpGet("GetUser")]
        public IActionResult GetUser([FromHeader] int userId)
        {
            try
            {
                User user = new User();
                if (userId > 0)
                {
                    user = _service.User.GetUserById(userId);
                    if (user != null)
                    {
                        UserResponse userResponse = new UserResponse
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            RoleId = user.RoleId,
                            Address = user.Address,
                            BirthDate = user.BirthDate,
                            DisplayName = user.DisplayName,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            Image = user.Image,
                            ImageThumb = user.ImageThumb,
                            LastName = user.LastName,
                            Mobile = user.Mobile,
                            NationalCode = user.NationalCode,
                            Phone = user.Phone
                        };
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "کاربر با موفقیت ارسال شد", Value = new { response = userResponse }, Error = new { } });

                    }
                    else
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.NotFound, Message = "کاربر یافت نشد", Value = new { }, Error = new { } });

                    }
                }
                else
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "کد کاربر وارد نشده است", Value = new { }, Error = new { } });

                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

        [HttpGet("GetProfile")]
        public IActionResult GetProfile()
        {
            try
            {
                var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                User user = _service.User.GetUserById(Convert.ToInt32(userId));
                if (user.IsActive == true)
                {
                    UserResponse userResponse = new UserResponse
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        RoleId = user.RoleId,
                        Address = user.Address,
                        BirthDate = user.BirthDate,
                        DisplayName = user.DisplayName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        Image = user.Image,
                        ImageThumb = user.ImageThumb,
                        LastName = user.LastName,
                        Mobile = user.Mobile,
                        NationalCode = user.NationalCode,
                        Phone = user.Phone
                    };
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "کاربر با موفقیت ارسال شد", Value = new { response = userResponse }, Error = new { } });
                }
                else
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "کاربر غیرفعال است", Value = new { }, Error = new { } });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

        [HttpPost("EditProfile")]
        public IActionResult EditProfile([FromBody] EditUserRequest Editeduser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "خطای ناشناخته", Value = new { }, Error = new { ErrorMsg = ModelState } });
                }
                if (!string.IsNullOrEmpty(Editeduser.FirstName) && !string.IsNullOrEmpty(Editeduser.LastName) && !string.IsNullOrEmpty(Editeduser.UserName) && !string.IsNullOrEmpty(Editeduser.NationalCode) && !string.IsNullOrEmpty(Editeduser.Email) && !string.IsNullOrEmpty(Editeduser.Phone) && !string.IsNullOrEmpty(Editeduser.Mobile))
                {
                    var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    User user = _service.User.GetUserById(Convert.ToInt32(userId));
                    if (user != null)
                    {
                        if (user.IsActive == false)
                        {
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "کاربر غیرفعال است", Value = new { }, Error = new { } });
                        }
                        if (user.Email != Editeduser.Email && user.UserName != Editeduser.UserName)
                        {
                            Tuple<bool, bool> validate = _service.User.CheckUserNameAndEmailExist(Editeduser.Email, Editeduser.UserName);
                            if (validate.Item1)
                            {
                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "ایمیل تکراری است", Value = new { }, Error = new { } });

                            }
                            else if (validate.Item2)
                            {
                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "نام کاربری تکراری است", Value = new { }, Error = new { } });

                            }
                        }
                        if (user.Email != Editeduser.Email)
                        {
                            Tuple<bool, bool> validate = _service.User.CheckUserNameAndEmailExist(Editeduser.Email, "");
                            if (validate.Item1)
                            {
                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "ایمیل تکراری است", Value = new { }, Error = new { } });

                            }
                        }
                        if (user.UserName != Editeduser.UserName)
                        {
                            Tuple<bool, bool> validate = _service.User.CheckUserNameAndEmailExist("", Editeduser.UserName);
                            if (validate.Item2)
                            {
                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "نام کاربری تکراری است", Value = new { }, Error = new { } });
                            }
                        }
                        user.FirstName = Editeduser.FirstName.FixText();
                        user.LastName = Editeduser.LastName.FixText();
                        user.UserName = Editeduser.UserName.FixText();
                        user.DisplayName = Editeduser.FirstName.FixText() + " " + Editeduser.LastName.FixText();
                        user.NationalCode = Editeduser.NationalCode.FixText();
                        user.Email = Editeduser.Email.FixText();
                        user.Address = Editeduser.Address;
                        user.Phone = Editeduser.Phone.FixText();
                        user.Mobile = Editeduser.Mobile.FixText();
                        user.BirthDate = Editeduser.BirthDate;
                        _service.User.EditUser(user);

                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "کاربر با موفقیت ویرایش شد", Value = new { }, Error = new { } });
                    }
                    else
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.MethodNotAllowed, Message = "کاربر مورد نظر یافت نشد", Value = new { }, Error = new { } });
                    }
                }
                else
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "اطلاعات وارد شده نامعتبر است", Value = new { }, Error = new { } });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest _singIn)
        {
            try
            {
                //----------------------------------------------------------------------------------Check parameters
                if (string.IsNullOrEmpty(_singIn.Username))
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.BadRequest,
                        Message = "نام کاربری یا ایمیل الزامی است",
                        Value = new { },
                        Error = new { }
                    });
                }
                if (string.IsNullOrEmpty(_singIn.Password))
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.BadRequest,
                        Message = "کلمه عبور الزامی است",
                        Value = new { },
                        Error = new { }
                    });
                }
                //----------------------------------------------------------------------------------Check parameters

                //----------------------------------------------------------------------------------Find User                


                User user = _service.User.LoginUser(_singIn.Username, _singIn.Password);
                if (user == null)
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.NotFound,
                        Message = "نام کاربری یا کلمه عبور نادرست است.",
                        Value = new { },
                        Error = new { }
                    });
                }
                if (user.IsActive == false)
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.MethodNotAllowed,
                        Message = "کاربر مورد نظر غیرفعال است.",
                        Value = new { },
                        Error = new { }
                    });
                }
                var token = _service.User.GenToken(user);
                var refreshToken = "";
                if (_singIn.RememberMe)
                {
                    Random random = new Random();
                    refreshToken = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz0123456789", 50).Select(s => s[random.Next(s.Length)]).ToArray());
                }
                user.RememberMe = _singIn.RememberMe;
                user.RefreshToken = refreshToken;
                _service.User.EditUser(user);

                LoginResponse login = new LoginResponse
                {
                    BirthDate = user.BirthDate,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Image = user.Image,
                    ImageThumb = user.ImageThumb,
                    LastName = user.LastName,
                    Mobile = user.Mobile,
                    NationalCode = user.NationalCode,
                    Phone = user.Phone,
                    RoleId = user.RoleId,
                    Token = token.AccessToken,
                    RefreshToken = refreshToken,
                    UserName = user.UserName
                };
                return Ok(new
                {
                    TimeStamp = DateTime.Now,
                    ResponseCode = HttpStatusCode.OK,
                    Message = "ورود با موفقیت انجام شد.",
                    Value = new { Response = login },
                    Error = new { }
                });
                //----------------------------------------------------------------------------------Find User
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }

        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest _refreshTokenRequest)
        {
            try
            {
                //----------------------------------------------------------------------------------Check parameters
                if (_refreshTokenRequest is null)
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.BadRequest,
                        Message = "داده های دریافتی معتبر نمی باشد",
                        Value = new { },
                        Error = new { }
                    });
                }
                if (string.IsNullOrEmpty(_refreshTokenRequest.RefreshToken))
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.BadRequest,
                        Message = "مقدار رفرش توکن الزامی است",
                        Value = new { },
                        Error = new { }
                    });
                }
                if (string.IsNullOrEmpty(_refreshTokenRequest.Username))
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.BadRequest,
                        Message = "نام کاربری الزامی است",
                        Value = new { },
                        Error = new { }
                    });
                }
                //----------------------------------------------------------------------------------Check parameters
                //----------------------------------------------------------------------------------Check Customer Exist
                User user = _service.User.GetUserByUsername(_refreshTokenRequest.Username);
                if (user == null)
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.NotFound,
                        Message = "نام کاربری یا کلمه عبور نادرست است.",
                        Value = new { },
                        Error = new { }
                    });
                }
                if (user.IsActive == false)
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.MethodNotAllowed,
                        Message = "کاربر مورد نظر غیرفعال است.",
                        Value = new { },
                        Error = new { }
                    });
                }
                if (user.RefreshToken != _refreshTokenRequest.RefreshToken || !user.RememberMe)
                {
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.MethodNotAllowed,
                        Message = "رفرش توکن نامعتبر است.",
                        Value = new { },
                        Error = new { }
                    });
                }
                var token = _service.User.GenToken(user);
                var refreshToken = "";
                Random random = new Random();
                refreshToken = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz0123456789", 50).Select(s => s[random.Next(s.Length)]).ToArray());
                user.RefreshToken = refreshToken;
                _service.User.EditUser(user);

                LoginResponse login = new LoginResponse
                {
                    BirthDate = user.BirthDate,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Image = user.Image,
                    ImageThumb = user.ImageThumb,
                    LastName = user.LastName,
                    Mobile = user.Mobile,
                    NationalCode = user.NationalCode,
                    Phone = user.Phone,
                    RoleId = user.RoleId,
                    Token = token.AccessToken,
                    RefreshToken = refreshToken,
                    UserName = user.UserName
                };
                return Ok(new
                {
                    TimeStamp = DateTime.Now,
                    ResponseCode = HttpStatusCode.OK,
                    Message = "توکن با موفقیت بازیابی شد.",
                    Value = new { Response = login },
                    Error = new { }
                });
                //----------------------------------------------------------------------------------Check Customer Exist
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    TimeStamp = DateTime.Now,
                    ResponseCode = HttpStatusCode.InternalServerError,
                    Message = "خطا داخلی سرور رخ داده است",
                    Value = new { },
                    Error = new { message = ex.Message }
                });
            }
        }
    }
}
