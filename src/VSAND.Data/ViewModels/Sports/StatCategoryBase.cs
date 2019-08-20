using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.Sports
{
    public class StatCategoryBase<T>
    {
        public string name { get; }
        public int sortOrder { get; }

        public List<T> stats { get; set; }
    }
}
