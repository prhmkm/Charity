using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using static CharityManagementBackend.Domain.DTOs.PaymentDTO;

namespace CharityManagementBackend.Api.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : Controller
    {
        IServiceWrapper _service;
        private readonly AppSettings _appSettings;

        public PaymentController(IServiceWrapper service, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }
        [HttpGet("Dashboard1")]
        public IActionResult Dashboard1()
        {
            try
            {
                List<Dashboard1Response> res = _service.Payment.LastPayments().OrderBy(o => o.TranDate).ToList();
                foreach (var item in res)
                {
                    item.TranDate = item.TranDate.Substring(0, 4) + "/" + item.TranDate.Substring(4, 2) + "/" + item.TranDate.Substring(6, 2);
                }
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "اطلاعات با موفقیت ارسال شد", Value = new { response = res }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }
        [HttpGet("Dashboard2")]
        public IActionResult Dashboard2()
        {
            try
            {
                List<Dashboard2Response> res = _service.Payment.PaymentsByName().OrderBy(o => o.ServiceName).ToList();
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "اطلاعات با موفقیت ارسال شد", Value = new { response = res }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }


    }
}

