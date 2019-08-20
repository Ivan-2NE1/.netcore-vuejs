using System.Collections.Generic;
using VSAND.Data.ViewModels.Teams;


namespace VSAND.Services.Files
{
    public interface IPDFService
    {
        byte[] GeneratePdf(string html, string title);
    }
}