using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Manage.Conferences
{
    public class ConferenceService : IConferenceService
    {
        private VSAND.Data.Repositories.IUnitOfWork _uow;
        public ConferenceService(VSAND.Data.Repositories.IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
        }

        public async Task<VsandConference> GetConferenceAsync(int ConferenceId)
        {
            return await _uow.Conferences.GetById(ConferenceId);
        }

        public async Task<ServiceResult<VsandConference>> DeleteConferenceAsync(int ConferenceId)
        {
            var oRet = new ServiceResult<VsandConference>();

            VsandConference remConference = await GetConferenceAsync(ConferenceId);
            await _uow.Conferences.Delete(remConference.ConferenceId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remConference;
                oRet.Success = true;
                oRet.Id = remConference.ConferenceId;

                //TODO: This is the layer that the cache engine should be invoked for Conferences (frequently used)
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandConference>> UpdateConferenceAsync(VsandConference chgConference)
        {
            var oRet = new ServiceResult<VsandConference>();

            var oCheckConference = await TryFindDuplicateConference(chgConference);
            if (oCheckConference != null)
            {
                oRet.Message = string.Format("A Conference ({0}) already exists with the same name.", oCheckConference.ConferenceId);
                return oRet;
            }

            _uow.Conferences.Update(chgConference);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgConference;
                oRet.Success = true;
                oRet.Id = chgConference.ConferenceId;

                //TODO: This is the layer that the cache engine should be invoked for Conferences (frequently used)
            }
            return oRet;
        }

        public async Task<ServiceResult<VsandConference>> AddConferenceAsync(VsandConference addConference)
        {
            var oRet = new ServiceResult<VsandConference>();

            var oCheckConference = await TryFindDuplicateConference(addConference);
            if (oCheckConference != null)
            {
                oRet.Message = string.Format("A Conference ({0}) already exists with the same name.", oCheckConference.ConferenceId);
                return oRet;
            }

            // we can do the insert, it won't create a duplicate
            await _uow.Conferences.Insert(addConference);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addConference;
                oRet.Success = true;
                oRet.Id = addConference.ConferenceId;

                //TODO: This is the layer that the cache engine should be invoked for Conferences (frequently used)
            }
            return oRet;
        }

        public async Task<List<VsandConference>> GetListAsync()
        {
            var oRet = await _uow.Conferences.List(null, x => x.OrderBy(o => o.Name));
            return oRet.ToList();
        }

        private async Task<VsandConference> TryFindDuplicateConference(VsandConference conference)
        {
            return await _uow.Conferences.Single(c => c.ConferenceId != conference.ConferenceId && c.Name == conference.Name);
        }
    }
}
