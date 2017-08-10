using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoCMS.Web.Contract.Extensions;

namespace RoCMS.Helpers
{
    public static class DateHelper
    {
        /// <summary>
        /// Finds the next date whose day of the week equals the specified day of the week.
        /// </summary>
        /// <param name="startDate">
        /// The date to begin the search.
        /// </param>
        /// <param name="desiredDay">
        /// The desired day of the week whose date will be returneed.
        /// </param>
        /// <returns>
        /// The returned date is on the given day of this week.
        /// If the given day is before given date, the date for the
        /// following week's desired day is returned.
        /// </returns>
        private static DateTime GetNextDateForDay(DateTime startDate, DayOfWeek desiredDay)
        {
            // (There has to be a better way to do this, perhaps mathematically.)
            // Traverse this week
            DateTime nextDate = startDate;
            while (nextDate.DayOfWeek != desiredDay)
                nextDate = nextDate.AddDays(1D);

            return nextDate;
        }

        public static DateTime CountdownTimerParse(string param)
        {
            DateTime now = DateTime.UtcNow.ApplySiteTimezone();
            DateTime date;
            switch (param)
            {
                case "MONDAY":
                    date = GetNextDateForDay(now, DayOfWeek.Monday);
                    break;
                case "TUESDAY":
                    date = GetNextDateForDay(now, DayOfWeek.Tuesday);
                    break;
                case "WEDNESDAY":
                    date = GetNextDateForDay(now, DayOfWeek.Wednesday);
                    break;
                case "THURSDAY":
                    date = GetNextDateForDay(now, DayOfWeek.Thursday);
                    break;
                case "FRIDAY":
                    date = GetNextDateForDay(now, DayOfWeek.Friday);
                    break;
                case "SATURDAY":
                    date = GetNextDateForDay(now, DayOfWeek.Saturday);
                    break;
                case "SUNDAY":
                    date = GetNextDateForDay(now, DayOfWeek.Sunday);
                    break;
                default:
                    date = DateTime.Parse(param);
                    break;
            }
            date = date.Date.AddDays(1);
            return date;
        }

        /// <summary>
        /// Вид: SUNDAY 10:05
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DateTime NextDateTimeParse(string param)
        {
            var @params = param.Split(' ');
            TimeSpan time = TimeSpan.Parse(@params[1]);
            DateTime now = DateTime.UtcNow.ApplySiteTimezone();
            DateTime res = CountdownTimerParse(@params[0]).AddDays(-1D);
            res = res.Add(time);
            if (res < now)
            {
                res = res.AddDays(7d);
            }
            return res;
        }
    }
}
