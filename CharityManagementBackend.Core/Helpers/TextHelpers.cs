using System;
using System.Collections.Generic;
using System.Text;

namespace CharityManagementBackend.Core.Helpers
{
    public static class TextHelpers
    {
        public static string FixText(this string value)
        {
            return value.Trim().ToLower();
        }
        public static string NotNull(this string value)
        {
            if (value != null)
            {
                return value;
            }
            else
            {
                return "";
            }
        }
        public static bool IsDigitsOnly(string str)
        {
           
            foreach (char c in str)
            {
                if (c < '0' || c > '9')      
                return false;
            }
            return true;
        }
    }

}
