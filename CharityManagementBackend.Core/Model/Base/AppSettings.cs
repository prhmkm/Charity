using System;
using System.Collections.Generic;
using System.Text;

namespace CharityManagementBackend.Core.Model.Base
{
    public class AppSettings
    {
        public string TokenSecret { get; set; }
        public int TokenValidateInMinutes { get; set; }
        public string BankLocalAccount { get; set; }
        public string BankShetabAccount { get; set; }
        public string BankAccount { get; set; }
        public string BankIban { get; set; }
        public string BankName { get; set; }
        public string BankLogo { get; set; }
    }
}
