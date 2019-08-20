using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels
{
    public class ListItem<T>
    {
        public T id { get; set; }
        public string name { get; set; }

        public ListItem()
        {

        }

        public ListItem(T id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
