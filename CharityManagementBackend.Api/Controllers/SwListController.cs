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
    public class SwListController : Controller
    {
        IServiceWrapper _service;
        private readonly AppSettings _appSettings;
        public SwListController(IServiceWrapper service, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }
        [HttpGet("SwList")]
        public IActionResult CharityList()
        {
            try
            {
                List<SwList> res = _service.SwList.GetAll();
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "اطلاعات با موفقیت ارسال شد", Value = new { response = res }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }
    }
}
