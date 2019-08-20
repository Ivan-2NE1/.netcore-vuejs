using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.ViewModels;
using VSAND.Data.Repositories;

namespace VSAND.Services.Data
{
    //TODO: Rethinking this. I thought it could be generic, but not so sure now
    public class GenericDataService<T> : IGenericDataService<T>
    {

        private IUnitOfWork _uow;

        public GenericDataService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
        }

        public ServiceResult<T> Delete(T remModel)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<T> Insert(T newModel)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<T> Update(T chgModel)
        {
            throw new NotImplementedException();
        }
    }
}
