using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels
{
    public class ServiceResult<T>
    {
        public bool Success = false;
        public int Id = 0;
        public string Message = "";
        public T obj = default;
    }
}
