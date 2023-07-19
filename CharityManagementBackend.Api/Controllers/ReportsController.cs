using CharityManagementBackend.Core.Helpers;
using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Domain.DTOs;
using CharityManagementBackend.Service.Base;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using static CharityManagementBackend.Domain.DTOs.PaymentDTO;

namespace CharityManagementBackend.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : Controller
    {
        IServiceWrapper _service;
        private readonly AppSettings _appSettings;
        public ReportsController(IServiceWrapper service, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }
        [HttpPost("Transaction")]
        public IActionResult Transaction([FromBody] ReportsDTO reports, [FromHeader] int pageSize, [FromHeader] int pageNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(reports.date1))
                {
                    var arr = reports.date1.Split("/");
                    reports.date1 = arr[0] + "" + arr[1].PadLeft(2, '0') + "" + arr[2].PadLeft(2, '0');
                    if (!TextHelpers.IsDigitsOnly(reports.date1.Replace("/", "")))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                    }
                }
                if (!string.IsNullOrEmpty(reports.date2))
                {
                    var arr = reports.date2.Split("/");
                    reports.date2 = arr[0] + "" + arr[1].PadLeft(2, '0') + "" + arr[2].PadLeft(2, '0');
                    if (!TextHelpers.IsDigitsOnly(reports.date2.Replace("/", "")))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                    }
                }
                if (!string.IsNullOrEmpty(reports.date1) && !string.IsNullOrEmpty(reports.date2) && Convert.ToInt32(reports.date1.Replace("/", "")) > Convert.ToInt32(reports.date2.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ شروع نباید بزرگتر از تاریخ پایان ‍‍باشد ", Value = new { }, Error = new { } });
                }
                var res = _service.Reports.Transaction(reports.date1, reports.date2, reports.TSRVCID, reports.SwCode, reports.TRPAN, pageSize, pageNumber);
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "اطلاعات با موفقیت ارسال شد", Value = new { max = _service.Reports.Transaction(reports.date1, reports.date2, reports.TSRVCID, reports.SwCode, reports.TRPAN).Count, response = res }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }
        [HttpPost("PaymentsReport")]
        public IActionResult PaymentsReports([FromBody] PaymentsReport reports, [FromHeader] int pageSize, [FromHeader] int pageNumber)
        {
            try
            {
                if (!string.IsNullOrEmpty(reports.date1))
                {
                    var arr = reports.date1.Split("/");
                    reports.date1 = arr[0] + "" + arr[1].PadLeft(2, '0') + "" + arr[2].PadLeft(2, '0');
                    if (!TextHelpers.IsDigitsOnly(reports.date1.Replace("/", "")))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                    }
                }
                if (!string.IsNullOrEmpty(reports.date2))
                {
                    var arr = reports.date2.Split("/");
                    reports.date2 = arr[0] + "" + arr[1].PadLeft(2, '0') + "" + arr[2].PadLeft(2, '0');
                    if (!TextHelpers.IsDigitsOnly(reports.date2.Replace("/", "")))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                    }
                }
                if (!string.IsNullOrEmpty(reports.date1) && !string.IsNullOrEmpty(reports.date2) && Convert.ToInt32(reports.date1.Replace("/", "")) > Convert.ToInt32(reports.date2.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ شروع نباید بزرگتر از تاریخ پایان ‍‍باشد ", Value = new { }, Error = new { } });
                }
                var res = _service.Payment.Reports(reports.date1, reports.date2, reports.TSRVCID, reports.SwCode, pageSize, pageNumber);
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.OK, Message = "اطلاعات با موفقیت ارسال شد", Value = new { max = _service.Payment.Reports(reports.date1, reports.date2, reports.TSRVCID, reports.SwCode).Count, response = res }, Error = new { } });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

        [HttpGet("TransactionReportToFile")]
        public IActionResult TransactionReportToFile([FromHeader] string fromDate, [FromHeader] string toDate)
        {
            try
            {
                if ((string.IsNullOrEmpty(fromDate)) || (string.IsNullOrEmpty(toDate)))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                }
                if (!string.IsNullOrEmpty(fromDate))
                {
                    var arr = fromDate.Split("/");
                    fromDate = arr[0] + "/" + arr[1].PadLeft(2, '0') + "/" + arr[2].PadLeft(2, '0');
                }
                if (!string.IsNullOrEmpty(toDate))
                {
                    var arr = toDate.Split("/");
                    toDate = arr[0] + "/" + arr[1].PadLeft(2, '0') + "/" + arr[2].PadLeft(2, '0');
                }
                if (Convert.ToInt32(fromDate.Replace("/", "")) > Convert.ToInt32(toDate.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ شروع نباید بزرگتر از تاریخ پایان ‍‍باشد ", Value = new { }, Error = new { } });
                }
                var res = _service.Payment.TranReports(fromDate, toDate);
                using (MemoryStream stream = new MemoryStream())
                {
                    var fileName = fromDate.Replace("/", "") + "-" + toDate.Replace("/", "") + ".xlsx";
                    var workbook = new XLWorkbook();
                    workbook.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    var worksheet = workbook.Worksheets.Add("Report");
                    worksheet.Style.NumberFormat.SetFormat("@");
                    worksheet.Range(worksheet.Cell("A1"), worksheet.Cell("F1")).Merge().Value = "گزارش ریز تراکنشهای سرویس پرداخت، از تاریخ " + fromDate + " تا تاریخ " + toDate;
                    worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    worksheet.Cell("A2").Value = "ردیف";
                    worksheet.Cell("B2").Value = "تاریخ - ساعت";
                    worksheet.Cell("C2").Value = "شماره پیگیری";
                    worksheet.Cell("D2").Value = "شماره ارجاع";
                    worksheet.Cell("E2").Value = "مبلغ";
                    worksheet.Cell("F2").Value = "نوع ترمینال";
                    var row = 3;
                    foreach (var item in res)
                    {
                        try
                        {
                            worksheet.Cell("A" + row).Value = item.Row;
                            worksheet.Cell("B" + row).Value = item.TranDateTime;
                            worksheet.Cell("C" + row).Value = item.TRTRACENO;
                            worksheet.Cell("D" + row).Value = item.TRRRN;
                            worksheet.Cell("E" + row).Value = item.TRAmount;
                            worksheet.Cell("F" + row).Value = item.FaName;
                            row++;
                        }
                        catch (Exception ex)
                        {
                            var x = ex.Message;
                            continue;
                        }
                    }
                    foreach (var item in worksheet.ColumnsUsed())
                    {
                        
                        item.AdjustToContents();
                    }

                    workbook.SaveAs(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    var file = File(
                       fileContents: stream.ToArray(),
                       contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       fileDownloadName: fileName
                   );
                    return Ok(new
                    {
                        TimeStamp = DateTime.Now,
                        ResponseCode = HttpStatusCode.OK,
                        Message = "فایل با موفقیت ارسال شد.",
                        Value = new { Response = file },
                        Error = new { }
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }

    }
}
