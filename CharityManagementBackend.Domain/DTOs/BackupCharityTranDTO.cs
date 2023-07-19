namespace CharityManagementBackend.Domain.DTOs
{
    public class BackupCharityTranDTO
    {
        public int Row { get; set; }
        public string ABRNCHCOD { get; set; } = null!;
        public string TRPOSCCOD { get; set; } = null!;
        public string TRANDATE { get; set; } = null!;
        public string TRANTIME { get; set; } = null!;
        public string TRTRACENO { get; set; } = null!;
        public string TRRRN { get; set; } = null!;
        public string TRPAN { get; set; } = null!;
        public decimal TRAmount { get; set; }
        public string TSRVCID { get; set; } = null!;
        public int SWCODE { get; set; }
        public string SwTitle { get; set; }
        public int Process { get; set; }
        public string ServiceName { get; set; }
        public string TrposcName { get; set; }
    }
}
