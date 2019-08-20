using System;

namespace VSAND.Services.Data
{
    public class ServiceBase
    {
        protected VSAND.Data.Repositories.IUnitOfWork _uow;

        public ServiceBase(VSAND.Data.Repositories.IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
        }
    }
}
