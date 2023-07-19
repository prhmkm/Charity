using CharityManagementBackend.Core.Helpers;
using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Data.Interface;
using CharityManagementBackend.Domain.DTOs;
using CharityManagementBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using static CharityManagementBackend.Domain.DTOs.CheckoutDTO;
using static CharityManagementBackend.Domain.DTOs.PaymentDTO;

namespace CharityManagementBackend.Data.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        charityContext _db;
        SampleContext _sampleDb;
        public PaymentRepository(charityContext repositoryPaymente, SampleContext sampleContext) : base(repositoryPaymente)
        {
            _db = repositoryPaymente;
            _sampleDb = sampleContext;
        }

        public List<CheckoutACC> Checkout(string TranDate1, string TranDate2, List<int> ServiceID)
        {
            return (from w in _db.Payments
                    join b in _db.CharityServices
                    on new { A = w.TSRVCID, w.SwCode } equals new { A = b.TSRVCID.ToString(), b.SwCode }
                    where (Convert.ToInt32(w.TranDate) >= Convert.ToInt32(TranDate1.Replace("/", ""))) && (Convert.ToInt32(w.TranDate) <= Convert.ToInt32(TranDate2.Replace("/", ""))) && ServiceID.Contains(Convert.ToInt32(w.TSRVCID)) && b.Account != "" && b.Account != "0" && b.Account != null
                    select new CheckoutACC { ServiceName = b.ServiceName, SumAmounts = w.SumAmounts, CharityAccount = b.Account, localPan = w.LocalPan, TranCount = w.TranCount, TSRVCID = b.TSRVCID, SWCODE = w.SwCode }).ToList();
        }

        public List<CheckoutAll> CheckoutAll(string TranDate1, string TranDate2, List<int> ServiceID)
        {
            return (from w in _db.Payments
                    join b in _db.CharityServices
                    on new { A = w.TSRVCID, w.SwCode } equals new { A = b.TSRVCID.ToString(), b.SwCode }
                    where (Convert.ToInt32(w.TranDate) >= Convert.ToInt32(TranDate1.Replace("/", ""))) && (Convert.ToInt32(w.TranDate) <= Convert.ToInt32(TranDate2.Replace("/", ""))) && ServiceID.Contains(Convert.ToInt32(w.TSRVCID))
                    select new CheckoutAll { TSRVCID = b.TSRVCID, ServiceName = b.ServiceName, SumAmounts = w.SumAmounts, CharityAccount = b.Account, localPan = w.LocalPan, TranCount = w.TranCount, CharityIban = b.Iban, SWCODE = w.SwCode }).ToList();
        }

        public List<CheckoutIBAN> CheckoutIban(string TranDate1, string TranDate2, List<int> ServiceID)
        {
            List<CheckoutIBAN> a = (from w in _db.Payments
                                    join b in _db.CharityServices
                                    on new { A = w.TSRVCID, w.SwCode } equals new { A = b.TSRVCID.ToString(), b.SwCode }
                                    where (Convert.ToInt32(w.TranDate) >= Convert.ToInt32(TranDate1.Replace("/", ""))) && (Convert.ToInt32(w.TranDate) <= Convert.ToInt32(TranDate2.Replace("/", ""))) && ServiceID.Contains(Convert.ToInt32(w.TSRVCID)) && b.Iban != "" && b.Iban != "0" && b.Iban != null
                                    select new CheckoutIBAN { ServiceName = b.ServiceName, SumAmounts = w.SumAmounts, CharityIban = b.Iban, localPan = w.LocalPan, TranCount = w.TranCount, TSRVCID = b.TSRVCID, SWCODE = w.SwCode }).ToList();

            return a;


        }

        //public List<CheckoutDTO> Checkout2(string TranDate1, string TranDate2, List<string> ServiceID)
        //{
        //        return (from w in _db.Payments
        //                group w by w.TSRVCID into f
        //                join b in _db.CharityServices on f.Key.TSRVCID equals b.TSRVCID.ToString()
        //                where(Convert.ToInt32(w.TranDate) >= Convert.ToInt32(TranDate1.Replace("/", ""))) && (Convert.ToInt32(w.TranDate) <= Convert.ToInt32(TranDate2.Replace("/", "")))
        //                select new CheckoutDTO { ServiceName = b.ServiceName, SumAmounts = w.SumAmounts, CharityAccount = b.Account, localPan = w.LocalPan, TranCount = w.TranCount
        //}).ToList();

        //}

        public bool ExistByTranDate(string tranDate)
        {
            return FindByCondition(w => w.TranDate == tranDate).Any();
        }
        public bool ExistByFromDateToDate(string fromdate, string todate)
        {
            return FindByCondition(w => Convert.ToInt32(w.TranDate.Replace("/", "")) >= Convert.ToInt32(fromdate.Replace("/", "")) && Convert.ToInt32(w.TranDate.Replace("/", "")) <= Convert.ToInt32(todate.Replace("/", ""))).Any();
        }

        public List<Dashboard1Response> LastPaymente()
        {
            return FindAll().OrderByDescending(o => o.TranDate).GroupBy(s => s.TranDate).Select(s => new Dashboard1Response { TranDate = s.Key, TranCount = s.Sum(o => o.TranCount) }).Take(30).ToList();
        }

        public List<Dashboard2Response> PaymentsByName()
        {
            string lastdate = FindAll().OrderByDescending(o => o.TranDate).GroupBy(s => s.TranDate).Select(s => s.Key).Take(30).Last().ToString(); //Take(30).Select(s=>new Dashboard2Response { ServiceName = s.Key.ServiceName , TranCount = s.Sum(o=>o.TranCount) } ).Take(30)/*.Where(s => (Convert.ToInt32(s.TranDate) >= Convert.ToInt32(DateHelpers.ToPersianDate(DateTime.Now.AddDays(-30), false, ""))) && (Convert.ToInt32(s.TranDate) <= Convert.ToInt32(DateHelpers.ToPersianDate(DateTime.Now, false, ""))))*/.ToList();
            //string firstdate = FindAll().OrderByDescending(o => o.TranDate).GroupBy(s => s.TranDate).Select(s => new { TranDate = s.Key }).First().ToString();      //Take(30).Select(s=>new Dashboard2Response { ServiceName = s.Key.ServiceName , TranCount = s.Sum(o=>o.TranCount) } ).Take(30)/*.Where(s => (Convert.ToInt32(s.TranDate) >= Convert.ToInt32(DateHelpers.ToPersianDate(DateTime.Now.AddDays(-30), false, ""))) && (Convert.ToInt32(s.TranDate) <= Convert.ToInt32(DateHelpers.ToPersianDate(DateTime.Now, false, ""))))*/.ToList(); 
            return _db.Payments.Where(s => Convert.ToInt32(s.TranDate.Replace("/", "")) >= Convert.ToInt32(lastdate.Replace("/", ""))).GroupBy(s => s.ServiceName).Select(s => new Dashboard2Response { ServiceName = s.Key, TranCount = s.Sum(o => o.TranCount) }).Take(30).ToList();

        }

        public List<PaymentsReports> Reports(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode)
        {
            List<string> li = new List<string>();
            List<string> la = new List<string>();
            foreach (var item in serviceID)
            {
                li.Add(item.ToString());
            }
            foreach (var item in SwCode)
            {
                la.Add(item.ToString());
            }

            var list = _db.Payments.Select(s => new
            {
                PaymentesId = s.PaymentesId,
                TranDate = s.TranDate,
                TSRVCID = s.TSRVCID,
                SwCode = s.SwCode,
                LocalPan = s.LocalPan,
                TranCount = s.TranCount,
                SumAmounts = s.SumAmounts,
                ServiceName = s.ServiceName
            }).Where(w => (string.IsNullOrEmpty(date1) ? true : Convert.ToInt32(w.TranDate) >= Convert.ToInt32(date1.Replace("/", ""))) &&
                     (string.IsNullOrEmpty(date2) ? true : Convert.ToInt32(w.TranDate) <= Convert.ToInt32(date2.Replace("/", ""))) &&
                     (serviceID.Count <= 0 ? true : li.Contains(w.TSRVCID)) &&
                     (SwCode.Count <= 0 ? true : la.Contains(w.SwCode.ToString())))
                     .GroupBy(s => new { s.ServiceName, s.TSRVCID, s.SwCode }).Select(s => new PaymentsReports { ServiceName = s.Key.ServiceName, SumAmounts = s.Sum(s => s.SumAmounts), TSRVCID = s.Key.TSRVCID, TranCount = s.Sum(s => s.TranCount), SwCode = s.Key.SwCode })
                     .ToList();
            return list.Select((s, index) => new PaymentsReports
            {
                PaymentesId = s.PaymentesId,
                TranDate = s.TranDate,
                TSRVCID = s.TSRVCID,
                SwCode = s.SwCode,
                LocalPan = s.LocalPan,
                TranCount = s.TranCount,
                SumAmounts = s.SumAmounts,
                ServiceName = s.ServiceName

            }).OrderBy(o => o.SwCode).ThenBy(o => o.TSRVCID).ToList();
        }

        public List<PaymentsReports> Reports(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, int pageSize, int pageNumber)
        {
            {
                List<string> li = new List<string>();
                List<string> la = new List<string>();
                foreach (var item in serviceID)
                {
                    li.Add(item.ToString());
                }
                foreach (var item in SwCode)
                {
                    la.Add(item.ToString());
                }

                var list = _db.Payments.Select(s => new
                {
                    PaymentesId = s.PaymentesId,
                    TranDate = s.TranDate,
                    TSRVCID = s.TSRVCID,
                    SwCode = s.SwCode,
                    LocalPan = s.LocalPan,
                    TranCount = s.TranCount,
                    SumAmounts = s.SumAmounts,
                    ServiceName = s.ServiceName
                }).Where(w => (string.IsNullOrEmpty(date1) ? true : Convert.ToInt32(w.TranDate) >= Convert.ToInt32(date1.Replace("/", ""))) &&
                         (string.IsNullOrEmpty(date2) ? true : Convert.ToInt32(w.TranDate) <= Convert.ToInt32(date2.Replace("/", ""))) &&
                         (serviceID.Count <= 0 ? true : li.Contains(w.TSRVCID)) &&
                         (SwCode.Count <= 0 ? true : la.Contains(w.SwCode.ToString())))
                         .GroupBy(s => new { s.ServiceName, s.TSRVCID, s.SwCode }).Select(s => new PaymentsReports { ServiceName = s.Key.ServiceName, SumAmounts = s.Sum(s => s.SumAmounts), TSRVCID = s.Key.TSRVCID, TranCount = s.Sum(s => s.TranCount), SwCode = s.Key.SwCode, SwTitle = _db.SwLists.FirstOrDefault(o => o.Id == s.Key.SwCode).SwTitle })
                         .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                return list.Select((s, index) => new PaymentsReports
                {

                    row = index + 1 + ((pageNumber - 1) * pageSize),
                    TranDate = s.TranDate,
                    TSRVCID = s.TSRVCID,
                    SwCode = s.SwCode,
                    SwTitle = s.SwTitle,
                    TranCount = s.TranCount,
                    SumAmounts = s.SumAmounts,
                    ServiceName = s.ServiceName

                }).OrderBy(o => o.SwCode).ThenBy(o => o.TSRVCID).ToList();
            }
        }

        public List<TranReportDTO> TranReports(string fromDate, string toDate)
        {
            _sampleDb.Database.SetCommandTimeout(1800000);
            return _sampleDb.sp_TranReport.FromSqlRaw("select ROW_NUMBER() over(order by BCT.TRANDATE,BCT.TRANTIME) Row ,substring(BCT.TRANDATE,1,4) + '/' + substring(BCT.TRANDATE,5,2) + + '/' + substring(BCT.TRANDATE,7,2) + ' ' + substring(BCT.TRANTIME,1,2) + ':' + substring(BCT.TRANTIME,3,2) + ':' + substring(BCT.TRANTIME,5,2) as TranDateTime , BCT.TRTRACENO, BCT.TRRRN, BCT.TRAmount, PC.FaName, BCT.SWCODE from BackupCharityTrans BCT inner join PosCondition PC on BCT.TRPOSCCOD = PC.PcCode where BCT.TRANDATE >= '" + fromDate.Replace("/", "") + "' and BCT.TRANDATE <= '" + toDate.Replace("/", "") + "' order by BCT.TRANDATE,BCT.TRANTIME").ToList();
        }
    }
}
