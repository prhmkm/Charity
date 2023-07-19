namespace CharityManagementBackend.Domain.DTOs
{
    public class ProcessingDTO
    {
        public class ProcessingListResponse
        {
            public string TRANDATE { get; set; }
            public string SWCODE { get; set; }
            public int CountTRANDATE { get; set; }

        }
        public class ProcessCharityTransRequest
        {
            public string TranDate { get; set; }
        }
        public class ProcessingCharityTransListResponse
        {
            public string TRANDATE1 { get; set; }
            public string TRANDATE2 { get; set; }
            public int CountTRANDATE { get; set; }

        }
        public class DeleteCharityTransRequest
        {
            public string TranDate { get; set; }
        }
        public class ProcesingCharityTransList
        {
            public string TranDate1 { get; set; }
            public string TranDate2 { get; set; }
        }
    }
}
