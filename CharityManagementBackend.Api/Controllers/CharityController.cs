using CharityManagementBackend.Core.Helpers;
using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using static CharityManagementBackend.Domain.DTOs.CharityDTO;

namespace CharityManagementBackend.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CharityController : Controller
    {
        IServiceWrapper _service;
        private readonly AppSettings _appSettings;
        public CharityController(IServiceWrapper service, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }
        [AllowAnonymous]
        [HttpPost("AddCharity")]
        public IActionResult AddCharity([FromBody] AddCharityRequest charity)
        {
            charity.Iban = charity.Iban.ToUpper();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "خطای ناشناخته", Value = new { }, Error = new { ErrorMsg = ModelState } });
                }
                if (!string.IsNullOrEmpty(charity.ServiceName))
                {
                    if (string.IsNullOrEmpty(charity.Account) && string.IsNullOrEmpty(charity.Iban))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "هر دو شماره حساب و شماره شبا فرستاده نشده است", Value = new { }, Error = new { } });
                    }
                    else
                    {
                        bool validate = _service.Charity.CheckServiceNameExist(charity.ServiceID);

                        if (validate)
                        {
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شناسه تکراری است", Value = new { }, Error = new { } });
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(charity.Account) && !string.IsNullOrEmpty(charity.Iban))
                            {
                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "هر دو شماره حساب و شماره شبا فرستاده شده است", Value = new { }, Error = new { } });
                            }
                            else
                            {
                                if (charity.Iban.Length != 26 && string.IsNullOrEmpty(charity.Account))
                                {
                                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره شبا نامعتبر است", Value = new { }, Error = new { } });
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(charity.Iban) && string.IsNullOrEmpty(charity.Account))
                                    {
                                        if (charity.Iban.Substring(0, 2) != "IR" || !TextHelpers.IsDigitsOnly(charity.Iban.Substring(2, 24)))
                                        {
                                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره شبا نامعتبر است", Value = new { }, Error = new { } });
                                        }
                                    }
                                    else if (string.IsNullOrEmpty(charity.Iban) && !string.IsNullOrEmpty(charity.Account))
                                    {
                                        if (!TextHelpers.IsDigitsOnly(charity.Account))
                                        {
                                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره حساب نامعتبر است", Value = new { }, Error = new { } });
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(charity.ContactNumber))
                                    {
                                        if (!TextHelpers.IsDigitsOnly(charity.ContactNumber))
                                        {
                                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره تلفن همراه نامعتبر است", Value = new { }, Error = new { } });
                                        }
                                        if (charity.ContactNumber.Length != 11)
                                        {
                                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره تلفن همراه نامعتبر است", Value = new { }, Error = new { } });
                                        }
                                    }
                                    if (charity.SwCode == null)
                                    {
                                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "کد سوییچ را وارد کنید", Value = new { }, Error = new { } });
                                    }
                                    if (string.IsNullOrEmpty(charity.Iban))
                                    {
                                        charity.Iban = null;
                                    }
                                    if (string.IsNullOrEmpty(charity.Account))
                                    {
                                        charity.Account = null;
                                    }
                                    if (string.IsNullOrEmpty(charity.ContactName))
                                    {
                                        charity.ContactName = null;
                                    }
                                    if (string.IsNullOrEmpty(charity.ContactNumber))
                                    {
                                        charity.ContactNumber = null;
                                    }
                                    if (_service.Charity.ExistByKey(charity.ServiceID, charity.SwCode.GetValueOrDefault()))
                                    {
                                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شناسه خیریه با این کد سوییچ تکراری است", Value = new { }, Error = new { } });
                                    }
                                    CharityService CharityCreated = new CharityService()
                                    {
                                        TSRVCID = charity.ServiceID,
                                        ServiceName = charity.ServiceName,
                                        Account = charity.Account,
                                        SwCode = charity.SwCode.GetValueOrDefault(),
                                        Iban = charity.Iban,
                                        ContactName = charity.ContactName,
                                        ContactNumber = charity.ContactNumber,
                                        IsActive = charity.IsActive
                                    };
                                    _service.Charity.AddCharity(CharityCreated);
                                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "خیریه با موفقیت ثبت شد", Value = new { }, Error = new { } });
                                }
                            }
                        }
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
        [HttpGet("CharityList")]
        //[AllowAnonymous]
        public IActionResult CharityList([FromHeader] int pageSize, [FromHeader] int pageNumber)
        {
            try
            {
                List<CharityListResponse> res = _service.Charity.CharityList(pageSize,pageNumber);
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "اطلاعات با موفقیت ارسال شد", Value = new { response = res, max  = _service.Charity.CharityList() }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }
        [HttpPost("CharityListBySwCode")]
        public IActionResult CharityListBySwCode([FromHeader] List<int>? SwCode)
        {
            try
            {
                List<CharityListBySwCodeDTO> res = _service.Charity.CharityListBySwCode(SwCode).Select(s=> new CharityListBySwCodeDTO { ServiceID = s.ServiceID, ServiceName = s.ServiceName, SwCode = s.SwCode}).ToList();
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "اطلاعات با موفقیت ارسال شد", Value = new { response = res }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }
        [HttpPost("DeleteCharity")]
        public IActionResult DeleteCharity([FromBody] DeleteCharityRequest charity)
        {
            try
            {
                CharityService charityService = _service.Charity.GetCharityById(charity.ServiceID, charity.SwCode);
                charityService.IsDelete = true;
                _service.Charity.UpdateCharity(charityService);
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "عملیات با موفقیت انجام شد", Value = new { }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }

        }
        [HttpPost("EditCharity")]
        public IActionResult UpdateCharity([FromBody] UpdateCharityRequest charity)
        {
            charity.Iban = charity.Iban.ToUpper();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.BadRequest, Message = "خطای ناشناخته", Value = new { }, Error = new { ErrorMsg = ModelState } });
                }
                if (!string.IsNullOrEmpty(charity.ServiceName))
                {
                    if (string.IsNullOrEmpty(charity.Account) && string.IsNullOrEmpty(charity.Iban))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "هر دو شماره حساب و شماره شبا فرستاده نشده است", Value = new { }, Error = new { } });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(charity.Account) && !string.IsNullOrEmpty(charity.Iban))
                        {
                            return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "هر دو شماره حساب و شماره شبا فرستاده شده است", Value = new { }, Error = new { } });
                        }
                        else
                        {
                            if (charity.Iban.Length != 26 && string.IsNullOrEmpty(charity.Account))
                            {
                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره شبا نامعتبر است", Value = new { }, Error = new { } });
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(charity.Iban) && string.IsNullOrEmpty(charity.Account))
                                {
                                    if (charity.Iban.Substring(0, 2) != "IR" || !TextHelpers.IsDigitsOnly(charity.Iban.Substring(2, 24)))
                                    {
                                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره شبا نامعتبر است", Value = new { }, Error = new { } });
                                    }
                                }
                                else if (string.IsNullOrEmpty(charity.Iban) && !string.IsNullOrEmpty(charity.Account))
                                {
                                    if (!TextHelpers.IsDigitsOnly(charity.Account))
                                    {
                                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره حساب نامعتبر است", Value = new { }, Error = new { } });
                                    }
                                }
                                if (!string.IsNullOrEmpty(charity.ContactNumber))
                                {
                                    if (!TextHelpers.IsDigitsOnly(charity.ContactNumber))
                                    {
                                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره تلفن همراه نامعتبر است", Value = new { }, Error = new { } });
                                    }
                                    if (charity.ContactNumber.Length != 11)
                                    {
                                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شماره تلفن همراه نامعتبر است", Value = new { }, Error = new { } });
                                    }
                                }
                                if (charity.SwCode == null)
                                {
                                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "کد سوییچ را وارد کنید", Value = new { }, Error = new { } });
                                }
                                if (string.IsNullOrEmpty(charity.Iban))
                                {
                                    charity.Iban = null;
                                }
                                if (string.IsNullOrEmpty(charity.Account))
                                {
                                    charity.Account = null;
                                }
                                if (string.IsNullOrEmpty(charity.ContactName))
                                {
                                    charity.ContactName = null;
                                }
                                if (string.IsNullOrEmpty(charity.ContactNumber))
                                {
                                    charity.ContactNumber = null;
                                }
                                CharityService charityService = _service.Charity.GetCharityById(charity.ServiceID, charity.SwCode.GetValueOrDefault());
                                if(charityService == null)
                                {
                                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "شناسه یا کد سوییچ نادرست است", Value = new { }, Error = new { } });
                                }
                                charityService.ServiceName = charity.ServiceName;
                                charityService.Account = charity.Account;
                                charityService.Iban = charity.Iban;
                                charityService.ContactName = charity.ContactName;
                                charityService.ContactNumber = charity.ContactNumber;
                                charityService.IsActive = charity.IsActive;
                                _service.Charity.UpdateCharity(charityService);
                                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "عملیات با موفقیت انجام شد", Value = new { }, Error = new { } });
                            }
                        }
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
    }

}

