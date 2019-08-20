using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Manage.Publications
{
    public class PublicationService : IPublicationService
    {
        protected VSAND.Data.Repositories.IUnitOfWork _uow;
        public PublicationService(VSAND.Data.Repositories.IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
        }

        public async Task<IEnumerable<ListItem<int>>> GetList()
        {
            // TODO: PublicationService GetList needs some caching
            var oRet = new List<ListItem<int>>();
            var oPubs = await _uow.Publications.List(null, x => x.OrderBy(p => p.Name));
            foreach(VSAND.Data.Entities.VsandPublication pub in oPubs)
            {
                oRet.Add(new ListItem<int>(pub.PublicationId, pub.Name));
            }
            return oRet;
        }
    }
}
