using CharityManagementBackend.Core.Helpers;
using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Service.Base;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Xml.Linq;
using static CharityManagementBackend.Domain.DTOs.CheckoutDTO;

namespace CharityManagementBackend.Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CheckoutController : Controller
    {
        IServiceWrapper _service;
        private readonly AppSettings _appSettings;
        public CheckoutController(IServiceWrapper service, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }
        [HttpGet("Checkout")]
        public IActionResult Checkout([FromHeader] string date1, [FromHeader] string date2, [FromHeader] List<int> ServiceID)
        {
            try
            {
                List<object> filesList = new List<object>();
                var fromDate = "";
                var toDate = "";
                if (!string.IsNullOrEmpty(date1))
                {
                    var arr = date1.Split("/");
                    date1 = arr[0] + "" + arr[1].PadLeft(2, '0') + "" + arr[2].PadLeft(2, '0');
                    fromDate = arr[0] + "/" + arr[1].PadLeft(2, '0') + "/" + arr[2].PadLeft(2, '0');
                    if (!TextHelpers.IsDigitsOnly(date1.Replace("/", "")))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                    }
                }
                if (!string.IsNullOrEmpty(date2))
                {
                    var arr = date2.Split("/");
                    date2 = arr[0] + "" + arr[1].PadLeft(2, '0') + "" + arr[2].PadLeft(2, '0');
                    toDate = arr[0] + "/" + arr[1].PadLeft(2, '0') + "/" + arr[2].PadLeft(2, '0');
                    if (!TextHelpers.IsDigitsOnly(date2.Replace("/", "")))
                    {
                        return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ نامعتبر است", Value = new { }, Error = new { } });
                    }
                }
                if (Convert.ToInt32(date1.Replace("/", "")) > Convert.ToInt32(date2.Replace("/", "")))
                {
                    return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.FailedDependency, Message = "تاریخ شروع نباید بزرگتر از تاریخ پایان ‍‍باشد ", Value = new { }, Error = new { } });
                }

                StringBuilder sb = new StringBuilder();
                var res = _service.Payment.Checkout(date1, date2, ServiceID).GroupBy(n => new { n.ServiceName, n.CharityAccount, n.TSRVCID }).Select(s => new CheckoutACC { ServiceName = s.Key.ServiceName, TranCount = s.Sum(s => s.TranCount), SumAmounts = s.Sum(s => s.SumAmounts), CharityAccount = s.Key.CharityAccount, SWCODE = s.Where(q => q.TSRVCID == s.Key.TSRVCID && q.CharityAccount == s.Key.CharityAccount).Select(s => s.SWCODE).FirstOrDefault() });
                var res4 = _service.Payment.CheckoutAll(date1, date2, ServiceID);
                if (res.Count() != 0)
                {
                    foreach (var item in res)
                    {
                        string acc = _appSettings.BankAccount;

                        sb.Append(
                            acc + ","
                            + item.SumAmounts + ","
                            + 0 + ","
                            + "تسویه تراکنش های " + item.ServiceName + " از " + date1.Replace("/", "") + " تا " + date2.Replace("/", "")
                            + "\r\n"
                            + item.CharityAccount + ","
                            + 0 + ","
                            + item.SumAmounts + ","
                            + "تسویه تراکنش های " + item.ServiceName + " از " + date1.Replace("/", "") + " تا " + date2.Replace("/", "")
                            + "\r\n");

                    }
                    byte[] fileContents = Encoding.UTF8.GetBytes(sb.ToString());
                    filesList.Add(File(fileContents, "text/csv", "Charity_" + fromDate.Replace("/", "") + "_TO_" + toDate.Replace("/", "") + ".csv"));
                }
                if (res.Count() != 0)
                {
                    sb = new StringBuilder();
                    sb.Append(
                            1 + ","
                            + res.Count() + ","
                            + res.Select(s => s.SumAmounts).Sum()
                            + "\r\n"
                            + _appSettings.BankAccount + ","
                            + res.Select(s => s.SumAmounts).Sum() + ","
                            + "D"
                            + "\r\n");
                    foreach (var item in res)
                    {
                        sb.Append(
                            item.CharityAccount + ","
                            + item.SumAmounts + ","
                            + "C" + ","
                            + "بابت کمک های مردمی  " + item.ServiceName + " مورخ " + date1.Replace("/", "") + " لغایت " + date2.Replace("/", "")
                            + "\r\n");
                    }
                    byte[] fileContentss = Encoding.UTF8.GetBytes(sb.ToString());
                    filesList.Add(File(fileContentss, "text/txt", "Charity_" + fromDate.Replace("/", "") + "_TO_" + toDate.Replace("/", "") + ".txt"));
                }

                var fileName = fromDate.Replace("/", "") + "-" + toDate.Replace("/", "") + ".xlsx";
                var fileName2 = "CharityReports_" + fromDate.Replace("/", "") + "-" + "To_" + toDate.Replace("/", "") + ".xlsx";

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
                var xlsRes = _service.Payment.TranReports(fromDate, toDate);
                if (xlsRes.Count() != 0)
                {
                    MemoryStream streamres = new MemoryStream();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        var workbook = new XLWorkbook();
                        workbook.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        var worksheet = workbook.Worksheets.Add("Report");
                        worksheet.Style.NumberFormat.SetFormat("@");
                        worksheet.RightToLeft = true;
                        string URL = _appSettings.BankLogo;
                        WebClient webClient = new WebClient();
                        byte[] imageData = webClient.DownloadData(URL);
                        MemoryStream objImage = new MemoryStream(imageData);
                        //worksheet.Range(worksheet.Cell("A1"), worksheet.Cell("F3")).Merge();
                        var image = worksheet.AddPicture(objImage).MoveTo(worksheet.Cell("D1"), 1, 20).Scale(0.5);
                        worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        worksheet.Range(worksheet.Cell("A4"), worksheet.Cell("G4")).Merge().Value = "گزارش ریز تراکنشهای سرویس پرداخت، از تاریخ " + fromDate + " تا تاریخ " + toDate;
                        worksheet.Cells("A5:G5").Style.Fill.SetBackgroundColor(XLColor.Yellow);
                        worksheet.Cells("A5:G5").Style.Font.SetFontColor(XLColor.Black);
                        worksheet.Cell("A5").Value = "ردیف";
                        worksheet.Cell("B5").Value = "کد سوییچ";
                        worksheet.Cell("C5").Value = "تاریخ - ساعت";
                        worksheet.Cell("D5").Value = "شماره پیگیری";
                        worksheet.Cell("E5").Value = "شماره ارجاع";
                        worksheet.Cell("F5").Value = "مبلغ";
                        worksheet.Cell("G5").Value = "نوع ترمینال";
                        var row = 6;
                        foreach (var item in xlsRes)
                        {
                            try
                            {
                                worksheet.Cell("A" + row).Value = item.Row;
                                worksheet.Cell("B" + row).Value = item.SWCODE;
                                worksheet.Cell("C" + row).Value = item.TranDateTime;
                                worksheet.Cell("D" + row).Value = item.TRTRACENO;
                                worksheet.Cell("E" + row).Value = item.TRRRN;
                                worksheet.Cell("F" + row).Value = item.TRAmount;
                                worksheet.Cell("F" + row).SetDataType(XLDataType.Number);
                                worksheet.Cell("G" + row).Value = item.FaName;

                                row++;
                            }
                            catch (Exception ex)
                            {
                                var x = ex.Message;
                                continue;
                            }
                        }
                        worksheet.Cells("F6:F" + (row - 1)).Style.NumberFormat.Format = "#,##0.00";
                        worksheet.Row(4).Height = 30;
                        worksheet.Columns("B:B").Width = 8;
                        worksheet.Columns("C:C").Width = 20;
                        worksheet.Columns("D:D").Width = 12;
                        worksheet.Columns("E:E").Width = 14;
                        worksheet.Columns("F:F").Width = 11;
                        worksheet.Columns("G:G").Width = 13;
                        worksheet.Range("A5:G" + (row - 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A5:G" + (row - 1)).Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                        worksheet.Range("A5:G" + (row - 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A5:G" + (row - 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A5:G" + (row - 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A5:G" + (row - 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        worksheet.Style.Font.SetFontName("B Nazanin");
                        worksheet.Style.Font.SetFontSize(12);
                        worksheet.Style.Font.SetFontCharSet(XLFontCharSet.Arabic);
                        //worksheet.Cells("F5:F" + (row - 1)).Style.NumberFormat = 
                        workbook.SaveAs(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        streamres = stream;
                    }

                    filesList.Add(File(
                       fileContents: streamres.ToArray(),
                       contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       fileDownloadName: fileName
                   ));
                }
                var res1 = _service.Payment.CheckoutIban(date1, date2, ServiceID).GroupBy(n => new { n.Trandate, n.ServiceName, n.CharityIban, n.TSRVCID }).Select(s => new CheckoutIBAN { Trandate = s.Key.Trandate, ServiceName = s.Key.ServiceName, TranCount = s.Sum(s => s.TranCount), SumAmounts = s.Sum(s => s.SumAmounts), CharityIban = s.Key.CharityIban, SWCODE = s.Where(q => q.TSRVCID == s.Key.TSRVCID && q.CharityIban == s.Key.CharityIban).Select(s => s.SWCODE).FirstOrDefault() });
                if (res1.Count() != 0)
                {
                    var xel = new List<XElement>();
                    foreach (var item in res1)
                    {
                        xel.Add(
                        new XElement(
                            new XElement("CdtTrfTxInf",
                                new XElement("PmtId",
                                    new XElement("InstrId", "EMPTY"),
                                    new XElement("EndToEndId", "0")
                                            ),
                                new XElement("Amt",
                                    new XElement("InstdAmt",
                                        new XAttribute("ccy", "IRR"), item.SumAmounts
                                                )
                                            ),
                                new XElement("Cdtr",
                                    new XElement("Nm", item.ServiceName)
                                            ),
                                new XElement("CdtrAcct",
                                    new XElement("Id",
                                        new XElement("IBAN", item.CharityIban)
                                                )
                                            ),
                                new XElement("RmtInf",
                                    new XElement("Ustrd", "بانک " + _appSettings.BankName + " از " + date1.Replace("/", "") + " تا " + date2.Replace("/", ""))
                                            )
                                         )
                            ));
                    }
                    XDocument doc = new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Document",
                        new XAttribute("Xmlns", "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03"),
                        new XElement("CstmrCdtTrfInitn",
                            new XElement("GrpHdr",
                                new XElement("MsgId", _appSettings.BankIban.Substring(2, 24) + "" + DateTime.Now.ToString("yyMMdd0HHmm")),
                                new XElement("CreDtTm", DateTime.Now),
                                new XElement("NbOfTxs", res1.Select(s => s.TranCount).Sum()),
                                new XElement("CtrlSum", res1.Select(s => s.SumAmounts).Sum()),
                                new XElement("InitgPty",
                                    new XElement("Nm", _appSettings.BankName)
                                            )
                                        ),
                            new XElement("PmtInf",
                                new XElement("PmtInfId", "1"),
                                new XElement("PmtMtd", "TRF"),
                                new XElement("NbOfTxs", res1.Select(s => s.TranCount).Sum()),
                                new XElement("CtrlSum", res1.Select(s => s.SumAmounts).Sum()),
                                new XElement("ReqdExctnDt", DateTime.Now),
                                new XElement("Dbtr",
                                    new XElement("Nm", _appSettings.BankName)
                                            ),
                                new XElement("DbtrAcct",
                                    new XElement("Id",
                                        new XElement("IBAN", _appSettings.BankIban)
                                                )
                                            ),
                                new XElement("DbtrAgt",
                                    new XElement("FinInstnId",
                                        new XElement("BIC", "BMJIIRTHXXX")
                                                )
                                            ),
                                new List<XElement>(xel)
                                        )
                                    )
                                )
                            );
                    byte[] fileContentsss = Encoding.UTF8.GetBytes(doc.ToString());
                    filesList.Add(File(fileContentsss, "text/Xml", _appSettings.BankIban.Substring(2, 24) + "" + DateTime.Now.ToString("yyMMddHHmmss") + ".ccti"));
                }
                if (res4.Count() != 0)
                {
                    sb = new StringBuilder();
                    foreach (var item in res4)
                    {
                        string acc = _appSettings.BankLocalAccount;
                        if (item.localPan == "1")
                        {
                            sb.Append(
                                acc + ","
                                + item.SumAmounts + ","
                                + 0 + ","
                                + "تسویه تراکنش های " + item.ServiceName + " از " + date1.Replace("/", "") + " تا " + date2.Replace("/", "")
                                + "\r\n"
                                + _appSettings.BankAccount + ","
                                + 0 + ","
                                + item.SumAmounts + ","
                                + "تسویه تراکنش های " + item.ServiceName + " از " + date1.Replace("/", "") + " تا " + date2.Replace("/", "")
                                + "\r\n"
                                );
                        }
                    }

                    byte[] fileContents1 = Encoding.UTF8.GetBytes(sb.ToString());
                    filesList.Add(File(fileContents1, "text/csv", "LocalChannel_" + fromDate.Replace("/", "") + "_TO_" + toDate.Replace("/", "") + ".csv"));
                }
                if (res4.Count() != 0)
                {
                    sb = new StringBuilder();
                    foreach (var item in res4)
                    {
                        string acc = _appSettings.BankShetabAccount;
                        if (item.localPan == "2")
                        {
                            sb.Append(
                                acc + ","
                                + item.SumAmounts + ","
                                + 0 + ","
                                + "تسویه تراکنش های " + item.ServiceName + " از " + date1.Replace("/", "") + " تا " + date2.Replace("/", "")
                                + "\r\n"
                                + _appSettings.BankAccount + ","
                                + 0 + ","
                                + item.SumAmounts + ","
                                + "تسویه تراکنش های " + item.ServiceName + " از " + date1.Replace("/", "") + " تا " + date2.Replace("/", "")
                                + "\r\n"
                                );
                        }
                    }

                    byte[] fileContents2 = Encoding.UTF8.GetBytes(sb.ToString());
                    filesList.Add(File(fileContents2, "text/csv", "ShetabChannel_" + fromDate.Replace("/", "") + "_TO_" + toDate.Replace("/", "") + ".csv"));
                }
                var sw = _service.SwList.GetAll();
                var xlsRess = _service.Payment.CheckoutAll(date1, date2, ServiceID).GroupBy(s => new { s.ServiceName, s.CharityIban, s.CharityAccount, s.SWCODE, s.TSRVCID }).Select(s => new CheckoutAll { ServiceName = s.Key.ServiceName, SumAmounts = s.Sum(s => s.SumAmounts), TranCount = s.Sum(s => s.TranCount), TSRVCID = s.Key.TSRVCID, CharityAccount = s.Key.CharityAccount, CharityIban = s.Key.CharityIban, SWCODE = s.Key.SWCODE });
                if (xlsRess.Count() != 0)
                {
                    MemoryStream streamress = new MemoryStream();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        var workbook = new XLWorkbook();
                        workbook.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        var worksheet = workbook.Worksheets.Add("Charity Report");
                        worksheet.Style.NumberFormat.SetFormat("@");
                        worksheet.RightToLeft = true;
                        worksheet.Range(worksheet.Cell("A1"), worksheet.Cell("G1")).Merge().Value = "گزارش تراکنشهای کمک های مردمی، از تاریخ " + fromDate + " تا تاریخ " + toDate;
                        worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        worksheet.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                        worksheet.Cells("A2:G2").Style.Fill.SetBackgroundColor(XLColor.Yellow);
                        worksheet.Cells("A2:G2").Style.Font.SetFontColor(XLColor.Black);
                        worksheet.Cell("A2").Value = "ردیف";
                        worksheet.Cell("B2").Value = "سوییچ";
                        worksheet.Cell("C2").Value = "کد سرویس";
                        worksheet.Cell("D2").Value = "نام خیریه";
                        worksheet.Cell("E2").Value = "تعداد تراکنش";
                        worksheet.Cell("F2").Value = "(مبلغ کل (ریال";
                        worksheet.Cell("G2").Value = "شماره حساب تسویه";

                        var row = 3;
                        int a = 0;
                        foreach (var item in xlsRess)
                        {
                            a++;
                            try
                            {
                                worksheet.Cell("A" + row).Value = a;
                                worksheet.Cell("B" + row).Value = sw.FirstOrDefault(w => w.Id == item.SWCODE)?.SwTitle;
                                worksheet.Cell("C" + row).Value = item.TSRVCID;
                                worksheet.Cell("D" + row).Value = item.ServiceName;
                                worksheet.Cell("E" + row).Value = item.TranCount;
                                worksheet.Cell("E" + row).SetDataType(XLDataType.Number);
                                worksheet.Cell("F" + row).Value = item.SumAmounts;
                                worksheet.Cell("F" + row).SetDataType(XLDataType.Number);
                                worksheet.Cell("G" + row).Value = string.IsNullOrEmpty(item.CharityIban) || item.CharityIban == "0" ? item.CharityAccount : item.CharityIban;
                                row++;
                            }
                            catch (Exception ex)
                            {
                                var x = ex.Message;
                                continue;
                            }
                        }
                        worksheet.Cells("F3:F" + (row - 1)).Style.NumberFormat.Format = "#,##0.00";
                        worksheet.Row(1).Height = 40;
                        worksheet.Range("A1:G" + (row - 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A1:G" + (row - 1)).Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                        worksheet.Range("A1:G" + (row - 1)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A1:G" + (row - 1)).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A1:G" + (row - 1)).Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        worksheet.Range("A1:G" + (row - 1)).Style.Border.TopBorder = XLBorderStyleValues.Thin;


                        foreach (var item in worksheet.ColumnsUsed())
                        {
                            item.AdjustToContents();
                        }
                        worksheet.Style.Font.SetFontName("B Nazanin");
                        //workbook.Range("F3:F" + (row - 1)).Style.Font.SetFontName("comma");
                        worksheet.Style.Font.SetFontSize(12);
                        worksheet.Style.Font.SetFontCharSet(XLFontCharSet.Arabic);
                        workbook.SaveAs(stream);
                        stream.Seek(0, SeekOrigin.Begin);
                        streamress = stream;
                    }

                    filesList.Add(File(
                       fileContents: streamress.ToArray(),
                       contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                       fileDownloadName: fileName2
                   ));
                }
                return Ok(new
                {
                    TimeStamp = DateTime.Now,
                    ResponseCode = HttpStatusCode.OK,
                    Message = "فایل با موفقیت ارسال شد.",
                    Value = new { Response = filesList },
                    Error = new { }
                });
            }
            catch (Exception ex)
            {
                return Ok(new { TimeStamp = DateTime.Now, ResponseCode = HttpStatusCode.InternalServerError, Message = "خطای داخلی سرور رخ داده است", Value = new { }, Error = new { Response = ex.ToString() } });
            }
        }
    }
}
