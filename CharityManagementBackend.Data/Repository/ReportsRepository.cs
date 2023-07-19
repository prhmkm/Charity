using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Data.Interface;
using CharityManagementBackend.Domain.DTOs;
using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Data.Repository
{
    public class ReportsRepository : BaseRepository<BackupCharityTran>, IReportsRepository
    {
        charityContext _db;
        public ReportsRepository(charityContext RepositoryContext) : base(RepositoryContext)
        {
            _db = RepositoryContext;
        }

        public List<BackupCharityTranDTO> Transaction(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, string? TranPan)
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
            var list = _db.BackupCharityTrans.Select(s => new
            {
                TRANTIME = s.TRANTIME,
                TRANDATE = s.TRANDATE,
                ABRNCHCOD = s.ABRNCHCOD,
                TRPOSCCOD = s.TRPOSCCOD,
                TRTRACENO = s.TRTRACENO,
                TRRRN = s.TRRRN,
                TRPAN = s.TRPAN,
                TRAmount = s.TRAmount,
                TSRVCID = s.TSRVCID,
                SWCODE = s.SWCODE,
                Process = s.Process,
                ServiceName = _db.Payments.FirstOrDefault(w => w.TSRVCID == s.TSRVCID) != null ? _db.Payments.FirstOrDefault(w => w.TSRVCID == s.TSRVCID).ServiceName : ""
            }).Where(w => (string.IsNullOrEmpty(date1) ? true : Convert.ToInt32(w.TRANDATE) >= Convert.ToInt32(date1.Replace("/", ""))) &&
                     (string.IsNullOrEmpty(date2) ? true : Convert.ToInt32(w.TRANDATE) <= Convert.ToInt32(date2.Replace("/", ""))) &&
                     (string.IsNullOrEmpty(TranPan) ? true : w.TRPAN.Contains(TranPan)) &&
                     (serviceID.Count <= 0 ? true : li.Contains(w.TSRVCID)) &&
                     (SwCode.Count <= 0 ? true : la.Contains(w.SWCODE.ToString())))
            .OrderBy(o => o.TRANDATE).OrderBy(o => o.TRANDATE).ThenBy(o => o.TRANTIME)

                    .ToList();
            return list.Select((s, index) => new BackupCharityTranDTO
            {
                TRANTIME = s.TRANTIME,
                TRANDATE = s.TRANDATE,
                ABRNCHCOD = s.ABRNCHCOD,
                TRPOSCCOD = s.TRPOSCCOD,
                TRTRACENO = s.TRTRACENO,
                TRRRN = s.TRRRN,
                TRPAN = s.TRPAN,
                TRAmount = s.TRAmount,
                TSRVCID = s.TSRVCID,
                SWCODE = s.SWCODE,
                Process = s.Process,
                ServiceName = s.ServiceName

            }).ToList();
        }

        public List<BackupCharityTranDTO> Transaction(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, string? TranPan, int pageSize, int pageNumber)
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
            var list = _db.BackupCharityTrans.Select(s => new
            {
                TRANTIME = s.TRANTIME,
                TRANDATE = s.TRANDATE,
                ABRNCHCOD = s.ABRNCHCOD,
                TRPOSCCOD = s.TRPOSCCOD,
                TRTRACENO = s.TRTRACENO,
                TRRRN = s.TRRRN,
                TRPAN = s.TRPAN,
                TRAmount = s.TRAmount,
                TSRVCID = s.TSRVCID,
                SWCODE = s.SWCODE,
                Process = s.Process,
                ServiceName = _db.Payments.FirstOrDefault(w => w.TSRVCID == s.TSRVCID) != null ? _db.Payments.FirstOrDefault(w => w.TSRVCID == s.TSRVCID).ServiceName : ""
            }).Where(w => (string.IsNullOrEmpty(date1) ? true : Convert.ToInt32(w.TRANDATE) >= Convert.ToInt32(date1.Replace("/", ""))) &&
                     (string.IsNullOrEmpty(date2) ? true : Convert.ToInt32(w.TRANDATE) <= Convert.ToInt32(date2.Replace("/", ""))) &&
                     (string.IsNullOrEmpty(TranPan) ? true : w.TRPAN.Contains(TranPan)) &&
                     (serviceID.Count <= 0 ? true : li.Contains(w.TSRVCID)) &&
                     (SwCode.Count <= 0 ? true : la.Contains(w.SWCODE.ToString()))).OrderBy(o => o.TRANDATE).ThenBy(o => o.TRANTIME)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return list.Select((s, index) => new BackupCharityTranDTO
            {
                Row = index + 1 + ((pageNumber - 1) * pageSize),
                TRANTIME = s.TRANTIME.Substring(0, 2) + ":" + s.TRANTIME.Substring(2, 2) + ":" + s.TRANTIME.Substring(4, 2),
                TRANDATE = s.TRANDATE.Substring(0, 4) + "/" + s.TRANDATE.Substring(4, 2) + "/" + s.TRANDATE.Substring(6, 2),
                ABRNCHCOD = s.ABRNCHCOD,
                TRPOSCCOD = s.TRPOSCCOD,
                TRTRACENO = s.TRTRACENO,
                TRRRN = s.TRRRN,
                TRPAN = s.TRPAN,
                TRAmount = s.TRAmount,
                TSRVCID = s.TSRVCID,
                SWCODE = s.SWCODE,
                SwTitle = _db.SwLists.FirstOrDefault(o => o.Id == s.SWCODE).SwTitle,
                Process = s.Process,
                ServiceName = s.ServiceName,
                TrposcName = _db.PosConditions.Where(o => o.PcCode == s.TRPOSCCOD).Select(o => o.FaName).FirstOrDefault()

            }).ToList();
        }
    }
}
