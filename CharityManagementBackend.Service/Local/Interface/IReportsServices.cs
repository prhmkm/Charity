using CharityManagementBackend.Domain.DTOs;
using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Service.Local.Interface
{
    public interface IReportsServices
    {
        List<BackupCharityTranDTO> Transaction(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, string? TranPan);
        List<BackupCharityTranDTO> Transaction(string date1, string date2, List<int> tSRVCID, List<int>? SwCode, string tRPAN, int pageSize, int pageNumber);
    }
}
