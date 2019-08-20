using System;

namespace VSAND.Common
{
    public class DateHelp
    {
        public enum TimeSpanType
        {
            Milliseconds = 0,
            Seconds = 1,
            Minutes = 2,
            Hours = 3,
            Days = 4,
            Weeks = 5,
            Months = 6,
            Years = 7
        }

        public static int LastDayOfMonth(int iMonth, int iYear = 0)
        {
            if (iYear == 0)
            {
                iYear = DateTime.Now.Year;
            }

            DateTime oTDate = new DateTime(iYear, iMonth, 1);
            oTDate = oTDate.AddMonths(1).AddDays(-1);
            return oTDate.Day;
        }

        public static DateTime LastDateOfMonth(DateTime dRef)
        {
            DateTime oTDate = new DateTime(dRef.Year, dRef.Month, 1);
            oTDate = oTDate.AddMonths(1).AddDays(-1);
            return oTDate;
        }

        public static DateTime FirstDayOfWeek(DateTime dReference)
        {
            return dReference.AddDays((int)dReference.DayOfWeek * -1);
        }

        public static DateTime FirstDayOfQuarter(DateTime dReference)
        {
            int iMonth = dReference.Month;
            switch (iMonth)
            {
                case 1:
                case 2:
                case 3:
                    {
                        iMonth = 1;
                        break;
                    }

                case 4:
                case 5:
                case 6:
                    {
                        iMonth = 4;
                        break;
                    }

                case 7:
                case 8:
                case 9:
                    {
                        iMonth = 7;
                        break;
                    }

                case 10:
                case 11:
                case 12:
                    {
                        iMonth = 10;
                        break;
                    }
            }

            // -- This is the last day of the quarter
            return new DateTime(dReference.Year, iMonth, 1);
        }

        public static DateTime LastDayOfQuarter(DateTime dReference)
        {
            int iMonth = dReference.Month;
            switch (iMonth)
            {
                case 1:
                case 2:
                case 3:
                    {
                        iMonth = 3;
                        break;
                    }

                case 4:
                case 5:
                case 6:
                    {
                        iMonth = 6;
                        break;
                    }

                case 7:
                case 8:
                case 9:
                    {
                        iMonth = 9;
                        break;
                    }

                case 10:
                case 11:
                case 12:
                    {
                        iMonth = 12;
                        break;
                    }
            }

            // -- This is the last day of the quarter
            return new DateTime(dReference.Year, iMonth, LastDayOfMonth(iMonth, dReference.Year));
        }

        public static double DateDiff(DateTime d1, DateTime d2, TimeSpanType Span)
        {
            TimeSpan oTS = d1.Subtract(d2);

            switch (Span)
            {
                case TimeSpanType.Milliseconds:
                    {
                        return oTS.TotalMilliseconds;
                    }

                case TimeSpanType.Seconds:
                    {
                        return oTS.Seconds;
                    }

                case TimeSpanType.Minutes:
                    {
                        return oTS.Minutes;
                    }

                case TimeSpanType.Hours:
                    {
                        return oTS.Hours;
                    }

                case TimeSpanType.Days:
                    {
                        return oTS.Days;
                    }

                case TimeSpanType.Weeks:
                    {
                        return oTS.Days / 7.0;
                    }

                case TimeSpanType.Years:
                    {
                        return d1.Year - d2.Year;
                    }
            }
            return 0;
        }

        public static string ArticulateDate(DateTime dDate)
        {
            TimeSpan oTs = System.DateTime.Now.Subtract(dDate);
            return ArticulateDate(oTs);
        }

        public static string ArticulateDate(TimeSpan oTs)
        {
            // return Cynosura.Articulate(oTs, Cynosura.TemporalGroupType.minute);
            throw new NotImplementedException("Need to find a replacement for Cynosura");
        }

        public static string GetPrettyDate(DateTime d, bool boldName, bool includeOriginalDate, string originalFormatString)
        {
            if (string.IsNullOrEmpty(originalFormatString))
            {
                originalFormatString = "dddd, dd MMMM yyyy";
            }
            string boldOpen = boldName ? "<b>" : "";
            string boldClose = boldName ? "</b>" : "";
            string sOriginal = includeOriginalDate ? " (" + d.ToString(originalFormatString) + ")" : "";
            // 1.
            // Get time span elapsed since the date. (Setup comparison so that it is time insensitive)
            DateTime dNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime dComp = new DateTime(d.Year, d.Month, d.Day);
            TimeSpan s = dNow.Subtract(dComp);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = System.Convert.ToInt32(s.TotalDays);

            // 3.
            // Get total number of seconds elapsed.
            // int secDiff = System.Convert.ToInt32(s.TotalSeconds);

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return boldOpen + d.ToString(originalFormatString) + boldClose;
            }

            string sRetFormatStr = boldOpen + "{0}" + sOriginal + boldClose;
            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                return string.Format(sRetFormatStr, "Today");
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return string.Format(sRetFormatStr, "Yesterday");
            }
            if (dayDiff <= 7)
            {
                return string.Format(sRetFormatStr, "Last " + d.ToString("dddd"));
            }
            if (dayDiff < 31)
            {
                int iDiff = (int)Math.Ceiling(System.Convert.ToDouble(dayDiff) / 7);
                string sPlural = "s";
                if (iDiff == 1)
                {
                    sPlural = "";
                }
                return string.Format(sRetFormatStr, string.Format("{0} week{1} ago", iDiff, sPlural));
            }
            return null;
        }

        public static string SeasonName()
        {
            string season = "";
            var dt = DateTime.Now;
            switch (dt.Month)
            {
                case 8:
                case 9:
                case 10:
                case 11:
                    season = "Fall";
                    break;
                case 12:
                case 1:
                case 2:
                case 3:
                    season = "Winter";
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    season = "Spring";
                    break;
            }
            return season;
        }

        public static DateTime SqlMinDate
        {
            get {
                return new DateTime(1753, 1, 1, 0, 0, 0);
            }
        }

        public static DateTime SqlMaxDate
        {
            get {
                return new DateTime(9999, 12, 31, 23, 59, 59);
            }
        }
    }
}
