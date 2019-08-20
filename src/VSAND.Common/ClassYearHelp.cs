using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Common
{
    public static class ClassYearHelp
    {
        public static string GraduationYearToClass(int? gradYr, int? checkYear)
        {
            if (!gradYr.HasValue && !checkYear.HasValue)
            {
                return "";
            }

            var graduationYear = 0;
            if (gradYr.HasValue)
            {
                graduationYear = gradYr.Value;
            }
            var contextYear = 0;
            if (checkYear.HasValue)
            {
                contextYear = checkYear.Value;
            }

            if (graduationYear == 0)
            {
                return "";
            }

            if (graduationYear == 0)
            {
                return "";
            }

            if (contextYear > graduationYear)
            {
                return $"Class of {graduationYear}";
            }

            if (contextYear == graduationYear)
            {
                return "Senior";
            }
            else if (contextYear + 1 == graduationYear)
            {
                return "Junior";
            }
            else if (contextYear + 2 == graduationYear)
            {
                return "Sophomore";
            }
            else if (contextYear + 3 == graduationYear)
            {
                return "Freshman";
            }
            else
            {
                return $"Class of {graduationYear}";
            }
        }

        public static string GraduationYearToClassAbbr(int? gradYr, int? checkYear)
        {
            if (!gradYr.HasValue && !checkYear.HasValue)
            {
                return "";
            }

            var graduationYear = 0;
            if (gradYr.HasValue)
            {
                graduationYear = gradYr.Value;
            }
            var contextYear = 0;
            if (checkYear.HasValue)
            {
                contextYear = checkYear.Value;
            }

            if (graduationYear == 0)
            {
                return "";
            }

            string shortYear = $"'{new DateTime(graduationYear, 1, 1).ToString("yy")}";

            if (contextYear > graduationYear)
            {
                return $"{shortYear}";
            }

            if (contextYear == graduationYear)
            {
                return "Sr";
            }
            else if (contextYear + 1 == graduationYear)
            {
                return "Jr";
            }
            else if (contextYear + 2 == graduationYear)
            {
                return "So";
            }
            else if (contextYear + 3 == graduationYear)
            {
                return "Fr";
            }
            else
            {
                return $"{shortYear}";
            }
        }
    }
}
