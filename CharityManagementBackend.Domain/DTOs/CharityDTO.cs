namespace CharityManagementBackend.Domain.DTOs
{
    public class CharityDTO
    {
        public class AddCharityRequest
        {
            public int ServiceID { get; set; }
            public int? SwCode { get; set; }
            public string? ServiceName { get; set; }
            public string? Account { get; set; }
            public string? Iban { get; set; }
            public string? ContactName { get; set; }
            public string? ContactNumber { get; set; }
            public bool? IsActive { get; set; }
        }
        public class CharityListResponse
        {
            public int ServiceID { get; set; }
            public bool? IsActive { get; set; }
            public string ServiceName { get; set; }
            public string? Account { get; set; }
            public string? Iban { get; set; }
            public string? ContactName { get; set; }
            public string? ContactNumber { get; set; }
            public int SwCode { get; set; }
            public string SwTitle { get; set; }
            public bool IsDelete { get; set; }

        }
        public class CharityListBySwCodeDTO
        {
            public int ServiceID { get; set; }
            public string ServiceName { get; set; }
            public int SwCode { get; set; }

        }
        public class DeleteCharityRequest
        {
            public int ServiceID { get; set; }
            public int SwCode { get; set; }
        }
        public class UpdateCharityRequest
        {
            public int ServiceID { get; set; }
            public int? SwCode { get; set; }
            public string? ServiceName { get; set; }
            public string? Account { get; set; }
            public string? Iban { get; set; }
            public string? ContactName { get; set; }
            public string? ContactNumber { get; set; }
            public bool? IsActive { get; set; }
        }
    }
}
