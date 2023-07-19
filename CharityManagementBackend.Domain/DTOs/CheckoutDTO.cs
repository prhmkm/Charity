namespace CharityManagementBackend.Domain.DTOs
{
    public class CheckoutDTO
    {
        public class CheckoutACC
        {
            
            public int TSRVCID { get; set; }
            public int SWCODE { get; set; }
            public string CharityAccount { get; set; }
            public decimal SumAmounts { get; set; }
            public string ServiceName { get; set; }
            public string localPan { get; set; }
            public int TranCount { get; set; }
        }
        public class CheckoutIBAN
        {
            
            public int TSRVCID { get; set; }
            public int SWCODE { get; set; }
            public string CharityIban { get; set; }
            public decimal SumAmounts { get; set; }
            public string ServiceName { get; set; }
            public string localPan { get; set; }
            public int TranCount { get; set; }
            public string Trandate { get; set; }
        }
        public class CheckoutAll
        {
            //public string BankLocalAccount { get; set; }
            public int TSRVCID { get; set; }
            public int SWCODE { get; set; }
            public string CharityAccount { get; set; }
            public string CharityIban { get; set; }
            public decimal SumAmounts { get; set; }
            public string ServiceName { get; set; }
            public string localPan { get; set; }
            public int TranCount { get; set; }
        }
    }
}
