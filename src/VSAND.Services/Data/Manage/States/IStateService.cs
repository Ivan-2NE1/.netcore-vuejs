using System.Collections.Generic;
using System.Threading.Tasks;
using VSAND.Data.Entities;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Manage.States
{
    public interface IStateService
    {
        Task<VsandState> GetStateAsync(int stateId);
        Task<ServiceResult<VsandState>> AddStateAsync(VsandState addState);
        Task<ServiceResult<VsandState>> UpdateStateAsync(VsandState chgState);
        Task<ServiceResult<VsandState>> DeleteStateAsync(int stateId);
        Task<List<VsandState>> GetListAsync();
        Task<List<ListItem<string>>> List();
    }
}
