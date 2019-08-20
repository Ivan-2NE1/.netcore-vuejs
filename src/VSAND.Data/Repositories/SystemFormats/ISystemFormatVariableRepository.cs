using System.Collections.Generic;
using VSAND.Data.Entities;
using VSAND.Data.Identity;

namespace VSAND.Data.Repositories
{
    interface ISystemFormatVariableRepository : IRepository<VsandSystemFormatVariable>
    {
        List<VsandSystemFormatVariable> GetFormatVariables();

        int AddFormatVariable(string VariableName, string VariableValue, string ValueType, ref string sMsg, ApplicationUser user);

        bool UpdateFormatVariable(int FormatVariableId, string VariableName, string VariableValue, string ValueType, ref string sMsg, ApplicationUser user);

        bool DeleteFormatVariable(int FormatVariableId, ref string sMsg, ApplicationUser user);
    }
}
