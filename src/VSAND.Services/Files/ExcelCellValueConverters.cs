using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace VSAND.Services.Files
{
    public static class ExcelCellValueConverters
    {
        #region String Converters
        public static int ToInt32(this string source)
        {
            int outNum;
            return int.TryParse(source, out outNum) ? outNum : 0;
        }
        public static int? ToInt32Nullable(this string source)
        {
            int outNum;
            return int.TryParse(source, out outNum) ? outNum : (int?)null;
        }
        public static decimal ToDecimal(this string source)
        {
            decimal outNum;
            return decimal.TryParse(source, out outNum) ? outNum : 0;
        }

        public static decimal? ToDecimalNullable(this string source)
        {
            decimal outNum;
            return decimal.TryParse(source, out outNum) ? outNum : (decimal?)null;
        }

        public static double ToDouble(this string source)
        {
            double outNum;
            return double.TryParse(source, out outNum) ? outNum : 0;
        }

        public static double? ToDoubleNullable(this string source)
        {
            double outNum;
            return double.TryParse(source, out outNum) ? outNum : (double?)null;
        }

        public static DateTime ToDateTime(this string source)
        {
            DateTime outDt;
            if (DateTime.TryParse(source, out outDt))
            {
                return outDt;
            }
            else
            {
                //Check OLE Automation date time
                if (IsNumeric(source))
                {
                    Double sourceTmp = 0;
                    Double.TryParse(source, out sourceTmp);
                    return DateTime.FromOADate(sourceTmp);
                }
                return DateTime.Now;
            }
        }

        public static DateTime? ToDateTimeNullable(this string source)
        {
            DateTime outDt;
            if (DateTime.TryParse(source, out outDt))
            {
                return outDt;
            }
            else
            {
                //Check and handle OLE Automation date time
                if (IsNumeric(source))
                {
                    Double sourceTmp = 0;
                    Double.TryParse(source, out sourceTmp);
                    return DateTime.FromOADate(sourceTmp);
                }
                return (DateTime?)null;
            }
        }

        public static bool ToBoolean(this string source)
        {
            if (!string.IsNullOrEmpty(source))
                if (source.ToLower() == "true" || source == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            else
            {
                return false;
            }
        }
        public static bool? ToBooleanNullable(this string source)
        {
            if (!string.IsNullOrEmpty(source))
                if (source.ToLower() == "true" || source == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            else
            {
                return (bool?)null;
            }
        }

        public static Guid ToGuid(this string source)
        {
            Guid outGuid;
            return Guid.TryParse(source, out outGuid) ? outGuid : Guid.Empty;
        }

        public static Guid? ToGuidNullable(this string source)
        {
            Guid outGuid;
            return Guid.TryParse(source, out outGuid) ? outGuid : (Guid?)null;
        }
        #endregion

        #region Util
        private static readonly Regex _isNumericRegex = new Regex("^(" +
            /*Hex*/ @"0x[0-9a-f]+" + "|" +
            /*Bin*/ @"0b[01]+" + "|" +
            /*Oct*/ @"0[0-7]*" + "|" +
            /*Dec*/ @"((?!0)|[-+]|(?=0+\.))(\d*\.)?\d+(e\d+)?" +
            ")$");
        static bool IsNumeric(string value)
        {
            return _isNumericRegex.IsMatch(value);
        }
        #endregion
    }
}
