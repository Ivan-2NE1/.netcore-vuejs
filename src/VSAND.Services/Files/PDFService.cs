using System.Text;
using System.Collections.Generic;
using VSAND.Data.ViewModels.Teams;
using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using NLog;

namespace VSAND.Services.Files
{
    public class PDFService : IPDFService
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly IConverter _converter;

        public PDFService(IConverter converter)
        {
            _converter = converter ?? throw new ArgumentException("Converter");
        }

        public byte[] GeneratePdf(string html, string title)
        {
            string cssPath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "sa4", "css", "bootstrap.css");
            Log.Info($"Trying to make a path: {cssPath}");
            if (!System.IO.File.Exists(cssPath))
            {
                cssPath = "";
            }

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.Letter,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = title
                },
                Objects = {
                    new ObjectSettings()
                    {
                        PagesCount = true,
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "uft-8", UserStyleSheet = cssPath},
                        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true},
                        FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = title }
                    }
                }
            };

            byte[] pdf = _converter.Convert(doc);

            return pdf;
        }
    }
}