using NLog;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;
using VSAND.Data.Identity;

namespace VSAND.Data.Repositories
{
    public class SystemFormatRepository : Repository<VsandSystemFormat>, ISystemFormatRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public SystemFormatRepository(VsandContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException("Context is null");
        }

        public VsandSystemFormat GetFormat(int FormatId)
        {
            VsandSystemFormat oRet = (from sf in _context.VsandSystemFormat
                                      where sf.FormatId == FormatId
                                      select sf).FirstOrDefault();

            return oRet;
        }

        public List<VsandSystemFormat> GetFormatters(string FormatType)
        {
            List<VsandSystemFormat> oRet = null;

            IEnumerable<VsandSystemFormat> oData = (from sf in _context.VsandSystemFormat
                                                    where sf.FormatType == FormatType
                                                    orderby sf.Name ascending
                                                    select sf);

            if (oData != null)
            {
                oRet = oData.ToList();
            }


            return oRet;
        }

        public int AddFormatter(string FormatType, string FormatClass, int SportId, string Name, string Description, string FileName, string FormatContents, ref string sMsg, ApplicationUser user)
        {
            string UserName = user.AppxUser.UserId;
            int UserId = user.AppxUser.AdminId;

            int FormatId = 0;

            VsandSystemFormat oFormat = (from f in _context.VsandSystemFormat
                                         where f.Sport.SportId == SportId && f.Name == Name
                                         select f).FirstOrDefault();

            if (oFormat == null)
            {
                oFormat = new VsandSystemFormat
                {
                    FormatType = FormatType,
                    Name = Name,
                    FileName = FileName,
                    FormatClass = FormatClass,
                    SportId = SportId,
                    Description = Description
                };

                _context.VsandSystemFormat.Add(oFormat);
                try
                {
                    _context.SaveChanges();

                    FormatId = oFormat.FormatId;
                }
                catch (Exception ex)
                {
                    sMsg = ex.Message;
                    Log.Error(ex, sMsg);
                }
            }
            else
            {
                sMsg = "A formatter already exists with the same name.";
            }


            if (FormatId > 0)
            {
                AuditRepository.AuditChange(_context, "vsand_SystemFormat", "FormatId", FormatId, "Insert", UserName, UserId);
                // -- Create the necessary directory structure and file to store this
                // TODO: decide what happens to formatters
                // string sFormatBase = HttpContext.Current.Server.MapPath("/app_data/Formatters/" + FormatType);

                throw new NotImplementedException("Need to fix");

                /*
                bool bHasDir = Directory.Exists(sFormatBase);
                if (!bHasDir)
                {
                    try
                    {
                        Directory.CreateDirectory(sFormatBase);
                        bHasDir = true;
                    }
                    catch (Exception ex)
                    {
                        sMsg = "Unable to create base directory for format type (" + FormatType + ").";
                        log.Error(sMsg, ex);
                    }
                }

                if (bHasDir)
                {
                    try
                    {
                        string sFilePath = Path.Combine(sFormatBase, FileName);

                        // -- Create format file
                        using (StreamWriter oSW = File.CreateText(sFilePath))
                        {
                            oSW.Write(FormatContents);
                        }

                        // TODO: audit file change, maybe?
                        // audithelp.AuditFileChange(sFilePath, "Created", UserName, UserId);
                    }
                    catch (Exception ex)
                    {
                        sMsg = "Unable to create format file.";
                        log.Error(sMsg, ex);
                    }
                }
                */
            }

            return FormatId;
        }

        public bool DeleteFormat(int FormatId, ref string sMsg, ApplicationUser user)
        {
            bool bRet = false;

            string UserName = user.AppxUser.UserId;
            int UserId = user.AppxUser.AdminId;

            VsandSystemFormat oFormat = (from f in _context.VsandSystemFormat.Include(sf => sf.PublicationSportSubscriptions)
                                         where f.FormatId == FormatId
                                         select f).FirstOrDefault();

            if (oFormat != null)
            {
                int iSubCount = oFormat.PublicationSportSubscriptions.Count;
                if (iSubCount == 0)
                {
                    AuditRepository.AuditChange(_context, "vsand_SystemFormat", "FormatId", FormatId, "Delete", UserName, UserId);
                    _context.VsandSystemFormat.Remove(oFormat);

                    try
                    {
                        _context.SaveChanges();

                        bRet = true;
                    }
                    catch (Exception ex)
                    {
                        sMsg = ex.Message;
                        Log.Error(ex, sMsg);
                    }
                }
                else
                {
                    sMsg = "The format cannot be removed. It is referenced in " + iSubCount + " publication sport subscription" + (iSubCount != 1 ? "s" : "") + ".";
                }
            }
            else
            {
                sMsg = "The format cannot be found. Perhaps it was already removed?";
            }

            return bRet;
        }
    }
}
