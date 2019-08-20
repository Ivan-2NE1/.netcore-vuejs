using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data
{
    interface IGenericDataService<T>
    {
        ServiceResult<T> Insert(T newModel);
        ServiceResult<T> Update(T chgModel);
        ServiceResult<T> Delete(T remModel);
    }
}
