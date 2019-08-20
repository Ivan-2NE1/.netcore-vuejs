using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;
using NLog;

namespace VSAND.Data.Repositories
{
    public class PublicationRepository : Repository<VsandPublication>, IPublicationRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public PublicationRepository(VsandContext context) : base(context)
        {
            _context = context;
        }
    }
}
