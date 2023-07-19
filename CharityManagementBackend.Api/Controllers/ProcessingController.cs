using CharityManagementBackend.Core.Helpers;
using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Service.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using static CharityManagementBackend.Domain.DTOs.ProcessingDTO;

namespace CharityManagementBackend.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProcessingController : Controller
    {
        IServiceWrapper _service;
        private readonly AppSettings _appSettings;
        public ProcessingController(IServiceWrapper service, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }
        [HttpGet("CharityTransList")]
        public IActionResult CharityTransList()
        {
            try
            {
                List<ProcessingListResponse> res = _service.Processing.Procesing().Select(s => new { TRANDATE = s.TRANDATE, SWCODE = s.SWCODE }).GroupBy(g => g.TRANDATE).Select(s => new ProcessingListResponse { TRANDATE = s.Key, SWCODE = string.Join(", ", s.Select(ss => ss.SWCODE).ToList().Distinct().ToList()), CountTRANDATE = s.Count() }).ToList();
                foreach (var item in res)
                {
                    item.TRANDATE = item.TRANDATE.Substring(0, 4) + "/" + item.TRANDATE.Substring(4, 2) + "/" + item.TRANDATE.Substring(6, 2);
                }
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "اطلاعات با موفقیت ارسال شد", Value = new { response = res }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }

        }
        [HttpPost("ProcessCharityTrans")]
        public IActionResult ProcessCharityTrans([FromBody] ProcessCharityTransRequest processCharityTrans)
        {
            try
            {
                var arr = processCharityTrans.TranDate.Split("/");
                processCharityTrans.TranDate = arr[0] + "" + arr[1].PadLeft(2, '0') + "" + arr[2].PadLeft(2, '0');
                if (!TextHelpers.IsDigitsOnly(processCharityTrans.TranDate.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                }
                else
                {

                    if (_service.Payment.ExistByTranDate(processCharityTrans.TranDate.Replace("/", "")))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.NotAcceptable, Message = "تراکنش های تاریخ " + processCharityTrans.TranDate + " قبلاً پردازش شده است", Value = new { }, Error = new { } });
                    }
                    else
                    {
                        _service.Processing.ProcessCharityTrans(processCharityTrans.TranDate.Replace("/", ""));
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "عملیات با موفقیت انجام شد",/* Value = new { response = res },*/ Error = new { } });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }

        }
        [HttpPost("DeleteCharityTrans")]
        public IActionResult DeleteCharityTrans([FromBody] DeleteCharityTransRequest DeleteCharityTrans)
        {
            try
            {
                var arr = DeleteCharityTrans.TranDate.Split("/");
                DeleteCharityTrans.TranDate = arr[0] + "" + arr[1].PadLeft(2, '0') + "" + arr[2].PadLeft(2, '0');
                if (!TextHelpers.IsDigitsOnly(DeleteCharityTrans.TranDate.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                }
                _service.Processing.DeleteCharityTrans(DeleteCharityTrans.TranDate.Replace("/", ""));
                _service.Save();
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "عملیات با موفقیت انجام شد",/* Value = new { response = res },*/ Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

        [HttpPost("ProcessingNewCharityTrans")]
        public IActionResult ProcesingCharityTransList([FromBody] ProcesingCharityTransList ProcessingNewCharityTrans)
        {
            try
            {
                var arr = ProcessingNewCharityTrans.TranDate1.Split("/");
                ProcessingNewCharityTrans.TranDate1 = arr[0] + "/" + arr[1].PadLeft(2, '0') + "/" + arr[2].PadLeft(2, '0');
                if (!TextHelpers.IsDigitsOnly(ProcessingNewCharityTrans.TranDate1.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                }
                var arr2 = ProcessingNewCharityTrans.TranDate2.Split("/");
                ProcessingNewCharityTrans.TranDate2 = arr2[0] + "/" + arr2[1].PadLeft(2, '0') + "/" + arr2[2].PadLeft(2, '0');
                if (!TextHelpers.IsDigitsOnly(ProcessingNewCharityTrans.TranDate2.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                }
                if (Convert.ToInt32(ProcessingNewCharityTrans.TranDate1.Replace("/", "")) > Convert.ToInt32(ProcessingNewCharityTrans.TranDate2.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ شروع نباید بزرگتر از تاریخ پایان ‍‍باشد ", Value = new { }, Error = new { } });
                }
                if (_service.Payment.ExistByFromDateToDate(ProcessingNewCharityTrans.TranDate1.Replace("/", ""),ProcessingNewCharityTrans.TranDate2.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.NotAcceptable, Message = "تراکنش های تاریخ " + ProcessingNewCharityTrans.TranDate1 + " تا " + ProcessingNewCharityTrans.TranDate2 + " قبلاً دریافت و پردازش شده است", Value = new { }, Error = new { } });
                }
                _service.Processing.ProcesingCharityTransList(DateHelpers.ToMiladiDateTime(ProcessingNewCharityTrans.TranDate1).GetValueOrDefault().ToString("yyyyMMdd"), DateHelpers.ToMiladiDateTime(ProcessingNewCharityTrans.TranDate2).GetValueOrDefault().ToString("yyyyMMdd"));
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "عملیات با موفقیت انجام شد",/*Value = new { response = res },*/ Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }
    }
}
