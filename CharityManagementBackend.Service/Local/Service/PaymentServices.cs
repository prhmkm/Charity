using CharityManagementBackend.Core.Model.Base;
using CharityManagementBackend.Data.Base;
using CharityManagementBackend.Domain.DTOs;
using CharityManagementBackend.Domain.Models;
using CharityManagementBackend.Service.Local.Interface;
using Microsoft.Extensions.Options;
using static CharityManagementBackend.Domain.DTOs.CheckoutDTO;
using static CharityManagementBackend.Domain.DTOs.PaymentDTO;

namespace CharityManagementBackend.Service.Local.Service
{
    public class PaymentServices : IPaymentServices
    {
        IRepositoryWrapper _repository;
        private readonly AppSettings _appSettings;
        public PaymentServices(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _repository = repository;
        }
        public List<Dashboard1Response> LastPayments()
        {
            return _repository.Payment.LastPaymente();
        }

        public List<Dashboard2Response> PaymentsByName()
        {
            return _repository.Payment.PaymentsByName();
        }

        public bool ExistByTranDate(string tranDate)
        {
            return _repository.Payment.ExistByTranDate(tranDate);
        }

        public List<CheckoutACC> Checkout(string TranDate1, string TranDate2, List<int> ServiceID)
        {
            return _repository.Payment.Checkout(TranDate1, TranDate2, ServiceID);
        }

        public List<PaymentsReports> Reports(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode)
        {
            return _repository.Payment.Reports(date1, date2, serviceID, SwCode);
        }
        public List<PaymentsReports> Reports(string? date1, string? date2, List<int>? serviceID, List<int>? SwCode, int pageSize, int pageNumber)
        {
            return _repository.Payment.Reports(date1, date2, serviceID, SwCode, pageSize, pageNumber);
        }

        public List<CheckoutIBAN> CheckoutIban(string TranDate1, string TranDate2, List<int> ServiceID)
        {
            return _repository.Payment.CheckoutIban(TranDate1, TranDate2, ServiceID);
        }

        public List<TranReportDTO> TranReports(string fromDate, string toDate)
        {
            return _repository.Payment.TranReports(fromDate, toDate);
        }

        public List<CheckoutAll> CheckoutAll(string TranDate1, string TranDate2, List<int> ServiceID)
        {
            return _repository.Payment.CheckoutAll(TranDate1, TranDate2, ServiceID);
        }

        public bool ExistByFromDateToDate(string fromdate, string todate)
        {
            return _repository.Payment.ExistByFromDateToDate(fromdate, todate);
        }
    }
}
