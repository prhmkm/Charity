using System;
using System.Globalization;

namespace CharityManagementBackend.Core.Helpers
{
    public static class DateHelpers
    {

        public static string ToPersianDate(this DateTime input, bool withTime = true, string dateSeperator = "/")
        {
            PersianCalendar pCal = new PersianCalendar();
            string returnValue = String.Format("{1:0000}{0}{2:00}{0}{3:00}", dateSeperator, pCal.GetYear(input), pCal.GetMonth(input), pCal.GetDayOfMonth(input));
            if (withTime) returnValue += String.Format(" {0:00}:{1:00}:{2:00}", input.Hour, input.Minute, input.Second);
            return returnValue;
        }
        public static string ToPersianDate(this DateTime? input, bool withTime = true, string dateSeperator = "/")
        {
            if (!input.HasValue) return String.Empty;
            return input.Value.ToPersianDate(withTime, dateSeperator);
        }

        public static DateTime? ToDateTime(this string input, string dateSeperator = "/")
        {
            if (input == null) return null;
            string[] dateValues = input.Split(dateSeperator);
            if (dateValues.Length != 3) return null;
            if (int.TryParse(dateValues[0], out int day) &&
                int.TryParse(dateValues[1], out int month) &&
                int.TryParse(dateValues[2], out int year))
            {
                PersianCalendar pCal = new PersianCalendar();
                return pCal.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            else return null;
        }

        public static DateTime? ToMiladiDateTime(this string input, string dateSeperator = "/")
        {
            if (input == null) return null;
            string[] dateValues = input.Split(dateSeperator);
            if (dateValues.Length != 3) return null;
            if (int.TryParse(dateValues[2], out int day) &&
                int.TryParse(dateValues[1], out int month) &&
                int.TryParse(dateValues[0], out int year))
            {
                PersianCalendar pCal = new PersianCalendar();
                return pCal.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            else return null;
        }

        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/"
                                     + pc.GetMonth(value).ToString("00") + "/" +
                                     pc.GetDayOfMonth(value).ToString("00");
        }

        public static string AddMonth(string date, double monthToAdd, string dateSeperator = "/")
        {
            var dateParts = date.Split("/");
            var day = dateParts[2];
            var month = Convert.ToInt32(dateParts[1]);
            var year = Convert.ToInt32(dateParts[0]);
            for (int i = 1; i <= monthToAdd; i++)
            {
                if (month == 12)
                {
                    year += 1;
                    month = 1;
                }
                else
                {
                    month += 1;
                }
            }
            return year + dateSeperator + month + dateSeperator + day;
        }
        public static string AddDays(string date, int daysToAdd, string dateSeperator = "/")
        {
            DateTime dateTime = ToMiladiDateTime(date).GetValueOrDefault();

            return ToPersianDate(dateTime.AddDays(daysToAdd),false);
        }

        public static double MonthDifference(DateTime d1, DateTime d2)
        {
            if (d1 > d2)
            {
                DateTime hold = d1;
                d1 = d2;
                d2 = hold;
            }

            int monthsApart = Math.Abs(12 * (d1.Year - d2.Year) + d1.Month - d2.Month) - 1;
            double daysInMonth1 = DateTime.DaysInMonth(d1.Year, d1.Month);
            double daysInMonth2 = DateTime.DaysInMonth(d2.Year, d2.Month);

            double dayPercentage = ((daysInMonth1 - d1.Day) / daysInMonth1)
                                  + (d2.Day / daysInMonth2);
            return (double)monthsApart + dayPercentage;
        }
    }
}
