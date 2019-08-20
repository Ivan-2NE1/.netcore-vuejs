using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VSAND.Backend.TagHelpers
{
    [HtmlTargetElement("audithistorydata")]
    public class AuditHistoryTable : TagHelper
    {
        private const string AuditTableAttributeName = "audit-table";
        private const string OriginalDataAttributeName = "original-data";
        private const string AdditionalCssClassesAtributeName = "additional-css-classes";


        public string AuditTable { get; set; }

        [HtmlAttributeName(OriginalDataAttributeName)]
        public string OriginalData { get; set; }

        [HtmlAttributeName(AdditionalCssClassesAtributeName)]
        public string AdditionalCssClasses { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "table";
            output.TagMode = TagMode.StartTagAndEndTag;

            if (!string.IsNullOrWhiteSpace(AdditionalCssClasses))
            {
                if (output.Attributes.Any(x => x.Name.ToLower() == "class"))
                {
                    AdditionalCssClasses = output.Attributes.First(x => x.Name.ToLower() == "class").Value + " " + AdditionalCssClasses;
                    output.Attributes.Remove(output.Attributes.First(x => x.Name.ToLower() == "class"));
                }
                // Add the additional CSS classes
                output.Attributes.Add("class", AdditionalCssClasses);
            }

            StringBuilder oSb = new StringBuilder();

            // need to parse the Xml Document that is "OriginalData" and output each Attr / Value as a row
            var dataXml = new XmlDocument();
            dataXml.LoadXml("<result>" + OriginalData + "</result>");
            var result = dataXml.SelectSingleNode("//result");
            var tableNode = result.SelectSingleNode(AuditTable);
            foreach(XmlNode childNode in tableNode.ChildNodes)
            {
                string sKey = childNode.Name;
                string sVal = childNode.InnerText;
                if (sVal.Contains("e+"))
                {
                    try
                    {
                        sVal = Double.Parse(sVal).ToString();
#pragma warning disable CS0168 // Variable is declared but never used
                    }
                    catch (Exception ex)
#pragma warning restore CS0168 // Variable is declared but never used
                    {
                        // don't do anything with this jammy
                    }
                }

                oSb.AppendLine("<tr>");
                oSb.AppendLine("<td>" + sKey + "</td>");
                oSb.AppendLine("<td>" + sVal + "</td>");
                oSb.AppendLine("</tr>");
            }

            output.Content.AppendHtml(oSb.ToString());

            base.Process(context, output);
        }
    }
}
