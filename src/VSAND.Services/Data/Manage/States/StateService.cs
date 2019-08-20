using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Manage.States
{
    public class StateService : IStateService
    {
        private VSAND.Data.Repositories.IUnitOfWork _uow;
        public StateService(VSAND.Data.Repositories.IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException("Unit of Work is null");
        }

        public async Task<VsandState> GetStateAsync(int stateId)
        {
            return await _uow.States.GetById(stateId);
        }

        public async Task<ServiceResult<VsandState>> DeleteStateAsync(int stateId)
        {
            var oRet = new ServiceResult<VsandState>();

            VsandState remState = await GetStateAsync(stateId);
            await _uow.States.Delete(remState.StateId);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = remState;
                oRet.Success = true;
                oRet.Id = remState.StateId;

                //TODO: This is the layer that the cache engine should be invoked for states (frequently used)
            }

            return oRet;
        }

        public async Task<ServiceResult<VsandState>> UpdateStateAsync(VsandState chgState)
        {
            var oRet = new ServiceResult<VsandState>();

            var oCheckState = await TryFindDuplicateState(chgState);
            if (oCheckState != null)
            {
                oRet.Message = string.Format("A state ({0}) already exists with the same name, abbreviation, or publication abbreviation.", oCheckState.StateId);
                return oRet;
            }

            _uow.States.Update(chgState);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = chgState;
                oRet.Success = true;
                oRet.Id = chgState.StateId;

                //TODO: This is the layer that the cache engine should be invoked for states (frequently used)
            }
            return oRet;
        }

        public async Task<ServiceResult<VsandState>> AddStateAsync(VsandState addState)
        {
            var oRet = new ServiceResult<VsandState>();

            var oCheckState = await TryFindDuplicateState(addState);
            if (oCheckState != null)
            {
                oRet.Message = string.Format("A state ({0}) already exists with the same name, abbreviation, or publication abbreviation.", oCheckState.StateId);
                return oRet;
            }

            // we can do the insert, it won't create a duplicate
            await _uow.States.Insert(addState);

            var bRet = await _uow.Save();

            if (bRet)
            {
                oRet.obj = addState;
                oRet.Success = true;
                oRet.Id = addState.StateId;

                //TODO: This is the layer that the cache engine should be invoked for states (frequently used)
            }
            return oRet;
        }

        public async Task<List<VsandState>> GetListAsync()
        {
            var oRet = await _uow.States.List(null, x => x.OrderBy(o => o.Name));
            return oRet.ToList();
        }

        private async Task<VsandState> TryFindDuplicateState(VsandState state)
        {
            return await _uow.States.Single(s => s.StateId != state.StateId && (s.Abbreviation == state.Abbreviation || s.PubAbbreviation == state.PubAbbreviation || s.Name == state.Name));
        }

        public async Task<List<ListItem<string>>> List()
        {
            var oStates = await _uow.States.List(null, x => x.OrderBy(o => o.Name));
            var oRet = (from s in oStates select new ListItem<string> { id = s.Abbreviation, name = s.Name }).ToList();
            return oRet;
        }
    }
}
