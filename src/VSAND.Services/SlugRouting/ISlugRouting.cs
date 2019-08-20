using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VSAND.Data.Entities;

namespace VSAND.Services.SlugRouting
{
    public interface ISlugRouting
    {
        Task<VsandEntitySlug> GetRoute(string path);
    }
}
