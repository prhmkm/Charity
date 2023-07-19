using CharityManagementBackend.Domain.DTOs;
using CharityManagementBackend.Domain.Models;
using static CharityManagementBackend.Domain.DTOs.CheckoutDTO;
using static CharityManagementBackend.Domain.DTOs.PaymentDTO;

namespace CharityManagementBackend.Service.Local.Interface
{
    public interface IPaymentServices
    {
        bool ExistByTranDate(string tranDate);
        bool ExistByFromDateToDate(string fromdate, string todate);
        List<Dashboard1Response> LastPayments();
        List<Dashboard2Response> PaymentsByName();
        List<CheckoutACC> Checkout(string TranDate1, string TranDate2, List<int> ServiceID);
        List<CheckoutAll> CheckoutAll(string TranDate1, string TranDate2, List<int> ServiceID);

        List<CheckoutIBAN> CheckoutIban(string TranDate1, string TranDate2, List<int> ServiceID);
        List<PaymentsReports> Reports(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode);
        List<PaymentsReports> Reports(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, int pageSize, int pageNumber);
        List<TranReportDTO> TranReports(string fromDate, string toDate);
    }
}
