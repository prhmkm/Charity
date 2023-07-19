using CharityManagementBackend.Service.Local.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharityManagementBackend.Service.Base
{
    public interface IServiceWrapper
    {
        IUserService User { get; }
        IPaymentServices Payment { get; }
        IProcessingServices Processing { get; }
        ICharityServices Charity { get; }
        IReportsServices Reports { get; }
        ISwListService SwList { get; }

        void Save();
    }
}
