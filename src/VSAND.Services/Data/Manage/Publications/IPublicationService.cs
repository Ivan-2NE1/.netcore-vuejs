using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.ViewModels;

namespace VSAND.Services.Data.Manage.Publications
{
    public interface IPublicationService
    {
        Task<IEnumerable<ListItem<int>>> GetList();
    }
}
