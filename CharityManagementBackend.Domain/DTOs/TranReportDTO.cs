using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityManagementBackend.Domain.DTOs
{
    public class TranReportDTO
    {
        public long Row { get; set; }
        public int SWCODE { get; set; }
        public string TranDateTime { get; set; }
        public string TRTRACENO { get; set; }
        public string TRRRN { get; set; }
        public decimal TRAmount { get; set; }
        public string FaName { get; set; }
    }
}
