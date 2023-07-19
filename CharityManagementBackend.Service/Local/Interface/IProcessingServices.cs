using CharityManagementBackend.Domain.Models;

namespace CharityManagementBackend.Service.Local.Interface
{
    public interface IProcessingServices
    {
        List<CharityTran> Procesing();
        void ProcessCharityTrans(string TranDate);
        void ProcesingCharityTransList(string TranDate1, string TranDate2);
        void DeleteCharityTrans(string TranDate);
        CharityTran GetCharityTranByDate(string TranDate);
    }
}
