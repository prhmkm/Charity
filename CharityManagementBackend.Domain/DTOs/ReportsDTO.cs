namespace CharityManagementBackend.Domain.DTOs
{
    public class ReportsDTO
    {
        public string date1 { get; set; } = null!;
        public string date2 { get; set; } = null!;
        public List<int> TSRVCID { get; set; } = null!;
        public string TRPAN { get; set; } = null!;
        public List<int>? SwCode { get; set; }

    }
}
