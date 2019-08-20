using System;
using System.ComponentModel;

namespace VSAND.Data.Entities
{
    public partial class VsandTeamCustomCode
    {
        public int CustomCodeId { get; set; }

        [DisplayName("Code Name")]
        public string CodeName { get; set; }

        [DisplayName("Code Value")]
        public string CodeValue { get; set; }

        public int TeamId { get; set; }

        public VsandTeam Team { get; set; }

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
