using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Manage.Counties
{
    public class CountyService : ICountyService
    {
        private VSAND.Data.Repositories.IUnitOfWork _uow;
        public CountyService(VSAND.Data.Repositories.IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
        }

        public async Task<VsandCounty> GetCountyAsync(int CountyId)
        {
            return await _uow.Counties.GetById(CountyId);
        }

        public async Task<IEnumerable<ListItem<int>>> GetList()
        {
            //TODO: CountyService -> GetList should have cache that expires when county is modified
            var oCounties = await _uow.Counties.List(null, x => x.OrderBy(s => s.Name));
            var oRet = (from c in oCounties select new ListItem<int> { id = c.CountyId, name = c.Name });
            return oRet;
        }

        public async Task<ServiceResult<VsandCounty>> DeleteCountyAsync(int countyId)
        {
            var oRet = new ServiceResult<VsandCounty>();

            VsandCounty remCounty = await GetCountyAsync(countyId);
            if (remCounty == null)
            {
                oRet.Success = false;
                oRet.Id = countyId;
                oRet.Message = "There were no counties found with Id " + countyId;
            }

            await _uow.Counties.Delete(remCounty.CountyId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remCounty;
                oRet.Success = true;
                oRet.Id = remCounty.CountyId;

                //TODO: This is the layer that the cache engine should be invoked for Counties (frequently used)
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandCounty>> UpdateCountyAsync(VsandCounty chgCounty)
        {
            var oRet = new ServiceResult<VsandCounty>();

            var oCheckCounty = await TryFindDuplicateCounty(chgCounty);
            if (oCheckCounty != null)
            {
                oRet.Message = string.Format("A County ({0}) already exists with the same name or abbreviation.", oCheckCounty.CountyId);
                return oRet;
            }

            _uow.Counties.Update(chgCounty);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgCounty;
                oRet.Success = true;
                oRet.Id = chgCounty.CountyId;

                //TODO: This is the layer that the cache engine should be invoked for Counties (frequently used)
            }
            return oRet;
        }

        public async Task<ServiceResult<VsandCounty>> AddCountyAsync(VsandCounty addCounty)
        {
            var oRet = new ServiceResult<VsandCounty>();

            var oCheckCounty = await TryFindDuplicateCounty(addCounty);
            if (oCheckCounty != null)
            {
                oRet.Message = string.Format("A County ({0}) already exists with the same name or abbreviation.", oCheckCounty.CountyId);
                return oRet;
            }

            // we can do the insert, it won't create a duplicate
            await _uow.Counties.Insert(addCounty);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addCounty;
                oRet.Success = true;
                oRet.Id = addCounty.CountyId;

                //TODO: This is the layer that the cache engine should be invoked for Counties (frequently used)
            }

            return oRet;
        }

        public async Task<List<VsandCounty>> GetListAsync()
        {
            var oRet = await _uow.Counties.List(null, x => x.OrderBy(o => o.Name));
            return oRet.ToList();
        }

        private async Task<VsandCounty> TryFindDuplicateCounty(VsandCounty county)
        {
            return await _uow.Counties.Single(c => c.CountyId != county.CountyId && (c.Name == county.Name || c.CountyAbbr == county.CountyAbbr));
        }
    }
}
