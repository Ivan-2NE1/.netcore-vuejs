using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;

namespace VSAND.Backend.Extensions
{
    public static class HtmlExtensions
    {
        private static readonly HtmlContentBuilder _emptyBuilder = new HtmlContentBuilder();

        public static IHtmlContent BuildBreadcrumbNavigation(this IHtmlHelper helper)
        {
            string controllerName = "";
            if (helper.ViewContext.RouteData.Values.ContainsKey("Container"))
            {
                controllerName = helper.ViewContext.RouteData.Values["Controller"].ToString();
            }

            var sDate = DateTime.Now.ToString("dddd, mmmm dd, yyyy");
            var sDateItem = string.Format("<li class='position-absolute pos-top pos-right d-none d-sm-block'><span class='js-get-date'>{0}</span></li>", sDate);

            if (controllerName == "Home")
            {
                return new HtmlContentBuilder()
                                .AppendHtml("<ol class='breadcrumb page-breadcrumb'>")
                                .AppendHtml("<li class='breadcrumb-item'>Dashboard</li>")
                                .AppendHtml(sDateItem)
                                .AppendHtml("</ol>");
            }

            var areaName = "";
            if (helper.ViewContext.RouteData.Values.ContainsKey("Area"))
            {
                areaName = helper.ViewContext.RouteData.Values["Area"].ToString();
            }
            
            var actionName = "";
            if (helper.ViewContext.RouteData.Values.ContainsKey("Action"))
            {
                actionName = helper.ViewContext.RouteData.Values["Action"].ToString();
            }
            
            var breadcrumb = new HtmlContentBuilder()
                                .AppendHtml("<ol class='breadcrumb page-breadcrumb'><li class='breadcrumb-item'>")
                                .AppendHtml(helper.ActionLink("Dashboard", "Index", "Home"))
                                .AppendHtml("</li>");

            if (areaName != "")
            {
                breadcrumb.AppendHtml("<li class='breadcrumb-item'>" + areaName.Titleize() + "</li>");
            }

            if (controllerName != "")
            {
                breadcrumb.AppendHtml("<li class='breadcrumb-item'>")
                    .AppendHtml(helper.ActionLink(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(controllerName), "Index", controllerName))
                    .AppendHtml("</li>");
            }

            if (actionName != "Index")
            {
                breadcrumb.AppendHtml("<li class='breadcrumb-item'>")
                          .AppendHtml(helper.ActionLink(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(actionName), actionName, controllerName))
                          .AppendHtml("</li>");
            }

            breadcrumb.AppendHtml(sDateItem);
            return breadcrumb.AppendHtml("</ol>");
        }
    }
}