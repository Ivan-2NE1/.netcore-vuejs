using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;
using VSAND.Data.Identity;

namespace VSAND.Data.Repositories
{
    class SystemFormatVariableRepository : Repository<VsandSystemFormatVariable>, ISystemFormatVariableRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public SystemFormatVariableRepository(VsandContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException("Context is null");
        }

        public List<VsandSystemFormatVariable> GetFormatVariables()
        {
            List<VsandSystemFormatVariable> oRet = null;

            IEnumerable<VsandSystemFormatVariable> oData = (from fv in _context.VsandSystemFormatVariable
                                                            orderby fv.VariableName ascending
                                                            select fv);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public int AddFormatVariable(string VariableName, string VariableValue, string ValueType, ref string sMsg, ApplicationUser user)
        {
            string UserName = user.AppxUser.UserId;
            int UserId = user.AppxUser.AdminId;
            int FormatVariableId = 0;

            VsandSystemFormatVariable oFormatVar = (from fv in _context.VsandSystemFormatVariable
                                                    where fv.VariableName == VariableName
                                                    select fv).FirstOrDefault();

            if (oFormatVar == null)
            {
                oFormatVar = new VsandSystemFormatVariable
                {
                    VariableName = VariableName,
                    VariableValue = VariableValue,
                    ValueType = ValueType
                };

                _context.VsandSystemFormatVariable.Add(oFormatVar);

                try
                {
                    _context.SaveChanges();
                    FormatVariableId = oFormatVar.FormatVariableId;
                }
                catch (Exception ex)
                {
                    sMsg = ex.Message;
                    Log.Error(ex, sMsg);
                }
            }
            else
            {
                sMsg = "A variable with the same name is already defined.";
            }

            if (FormatVariableId > 0)
            {
                AuditRepository.AuditChange(_context, "vsand_SystemFormatVariable", "FormatVariableId", FormatVariableId, "Insert", UserName, UserId);
            }

            return FormatVariableId;
        }

        public bool UpdateFormatVariable(int FormatVariableId, string VariableName, string VariableValue, string ValueType, ref string sMsg, ApplicationUser user)
        {
            bool bRet = false;

            string UserName = user.AppxUser.UserId;
            int UserId = user.AppxUser.AdminId;

            AuditRepository.AuditChange(_context, "vsand_SystemFormatVariable", "FormatVariableId", FormatVariableId, "Update", UserName, UserId);

            VsandSystemFormatVariable oFormatVar = (from fv in _context.VsandSystemFormatVariable
                                                    where fv.FormatVariableId == FormatVariableId
                                                    select fv).FirstOrDefault();

            if (oFormatVar != null)
            {
                int iExists = (from fv in _context.VsandSystemFormatVariable
                               where fv.VariableName == VariableName && fv.FormatVariableId != FormatVariableId
                               select fv).Count();

                if (iExists == 0)
                {
                    oFormatVar.VariableName = VariableName;
                    oFormatVar.VariableValue = VariableValue;
                    oFormatVar.ValueType = ValueType;

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
                    sMsg = "A variable with the same name is already defined.";
                }
            }
            else
            {
                sMsg = "The variable no longer exists. Perhaps it was deleted?";
            }

            return bRet;
        }

        public bool DeleteFormatVariable(int FormatVariableId, ref string sMsg, ApplicationUser user)
        {
            bool bRet = false;

            string UserName = user.AppxUser.UserId;
            int UserId = user.AppxUser.AdminId;

            AuditRepository.AuditChange(_context, "vsand_SystemFormatVariable", "FormatVariableId", FormatVariableId, "Delete", UserName, UserId);

            VsandSystemFormatVariable oFormatVar = (from fv in _context.VsandSystemFormatVariable
                                                    where fv.FormatVariableId == FormatVariableId
                                                    select fv).FirstOrDefault();

            if (oFormatVar != null)
            {
                _context.VsandSystemFormatVariable.Remove(oFormatVar);

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
                sMsg = "The variable was already been removed.";
            }

            return bRet;
        }
    }
}
