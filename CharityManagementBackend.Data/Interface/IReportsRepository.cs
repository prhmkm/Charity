using CharityManagementBackend.Domain.DTOs;

namespace CharityManagementBackend.Data.Interface
{
    public interface IReportsRepository
    {
        List<BackupCharityTranDTO> Transaction(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, string? TranPan);
        List<BackupCharityTranDTO> Transaction(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, string? TranPan, int pageSize, int pageNumber);
    }
}

