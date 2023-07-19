using CharityManagementBackend.Data.Interface;


namespace CharityManagementBackend.Data.Base
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }  
        IPaymentRepository Payment { get; }
        IProcessingRepository Processing { get; }
        ICharityRepository Charity { get; }
        IReportsRepository Reports { get; }
        ISwListRepository SwList { get; }
        void Save();
    }
}
