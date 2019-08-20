using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Teams
{
    public class TeamCustomCode
    {
        public int CustomCodeId { get; set; }
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
        public int TeamId { get; set; }

        public TeamCustomCode(VsandTeamCustomCode customCode)
        {
            CustomCodeId = customCode.CustomCodeId;
            CodeName = customCode.CodeName;
            CodeValue = customCode.CodeValue;
            TeamId = customCode.TeamId;
        }

        // Interacting with code values
        public T GetValue<T>()
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    // Cast ConvertFromString(string text) : object to (T)
                    return (T)converter.ConvertFromString(CodeValue);
                }
                return default;
            }
            catch (NotSupportedException)
            {
                return default;
            }
        }
    }
}
