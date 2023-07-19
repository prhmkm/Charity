namespace CharityManagementBackend.Domain.DTOs
{
    public class PaymentDTO
    {
        public class Dashboard1Response
        {
            public string TranDate { get; set; }
            public int TranCount { get; set; }
        }
        public class Dashboard2Response
        {
            public string ServiceName { get; set; }
            public int TranCount { get; set; }
        }
        public class PaymentsReport
        {
            public string date1 { get; set; }
            public string date2 { get; set; }
            public List<int> TSRVCID { get; set; } = null!;
            public List<int>? SwCode { get; set; }
        }
        public class PaymentsReports
        {
            public int row { get; set; }
            public int PaymentesId { get; set; }
            public string TranDate { get; set; } = null!;
            public string TSRVCID { get; set; } = null!;
            public int SwCode { get; set; }
            public string SwTitle { get; set; }
            public string? ServiceName { get; set; }
            public string LocalPan { get; set; } = null!;
            public int TranCount { get; set; }
            public decimal SumAmounts { get; set; }
        }

    }
}
