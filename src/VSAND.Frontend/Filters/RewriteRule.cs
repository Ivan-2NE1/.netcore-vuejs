using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSAND.Frontend.Filters
{
    public class RewriteRule
    {
        public string PathMatchExpression { get; set; }
        public string PathReplaceExpression { get; set; }

        public RewriteRule()
        {

        }

        public RewriteRule(string matchExpression, string replaceExpression)
        {
            PathMatchExpression = matchExpression;
            PathReplaceExpression = replaceExpression;
        }
    }
}
