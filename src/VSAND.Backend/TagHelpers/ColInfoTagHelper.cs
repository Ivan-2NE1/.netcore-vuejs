using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VSAND.Backend.TagHelpers
{
    [HtmlTargetElement("col-info")]
    public class ColInfoTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-for")]
        public ModelExpression Model { get; set; }

        [HtmlAttributeName("variable-name")]
        public string VariableName { get; set; } = "colInfo";

        [HtmlAttributeName("columns")]
        public string ColumnOrder { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(ColumnOrder))
            {
                throw new ArgumentException("Column order is required.");
            }

            base.Process(context, output);

            output.TagName = "script";
            output.Attributes.Add("type", "text/javascript");

            var aColumnOrder = ColumnOrder.Split(new char[] { ',', ';' });
            var columns = new List<ColumnInfo>();

            var resolver = new CamelCasePropertyNamesContractResolver();
            foreach (string column in aColumnOrder)
            {
                // Trim the value in case someone uses ", " or "; " as delim
                var sCol = column.Trim();
                ModelExplorer property = Model.ModelExplorer.Properties.FirstOrDefault(p => sCol == p.Metadata.PropertyName);
                if (property == null)
                {
                    continue;
                }

                var jsonPropName = resolver.GetResolvedPropertyName(property.Metadata.PropertyName);
                var colInfo = new ColumnInfo
                {
                    DisplayName = property.Metadata.DisplayName ?? property.Metadata.PropertyName,
                    ColumnName = jsonPropName
                };

                // populate colInfo from the validation decorators
                foreach (var validatorMetadata in property.Metadata.ValidatorMetadata)
                {
                    if (validatorMetadata is RequiredAttribute)
                    {
                        colInfo.Required = true;
                    }

                    if (validatorMetadata is MaxLengthAttribute)
                    {
                        colInfo.MaxLength = ((MaxLengthAttribute)validatorMetadata).Length;
                    }
                }

                columns.Add(colInfo);
            }

            output.Content.SetHtmlContent("window." + VariableName + " = " + JsonConvert.SerializeObject(columns));
        }

        private class ColumnInfo
        {
            public string DisplayName { get; set; }
            public string ColumnName { get; set; }
            public bool Required { get; set; }
            public int MaxLength { get; set; }
        }
    }
}
