using System.Collections.Generic;
using VSAND.Data.Entities;
using VSAND.Data.Identity;

namespace VSAND.Data.Repositories
{
    interface ISystemFormatRepository : IRepository<VsandSystemFormat>
    {
        VsandSystemFormat GetFormat(int FormatId);

        List<VsandSystemFormat> GetFormatters(string FormatType);

        int AddFormatter(string FormatType, string FormatClass, int SportId, string Name, string Description, string FileName, string FormatContents, ref string sMsg, ApplicationUser user);

        bool DeleteFormat(int FormatId, ref string sMsg, ApplicationUser user);
    }
}
