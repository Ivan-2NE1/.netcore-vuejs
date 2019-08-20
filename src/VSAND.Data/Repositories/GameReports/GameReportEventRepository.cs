using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.Repositories
{
    public class GameReportEventRepository : Repository<VsandGameReportEvent>, IGameReportEventRepository
    {
        private readonly Logger Log = LogManager.GetCurrentClassLogger();

        private readonly VsandContext _context;
        public GameReportEventRepository(VsandContext context) : base(context)
        {
            _context = context;
        }

        /*
        public List<VsandSportEvent> GetInactiveEvents(int GameReportId)
        {
            List<VsandSportEvent> oRet = null;

            IEnumerable<VsandSportEvent> oData = oDb.GameReportInActiveEvents(GameReportId);

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }

        public List<VsandSportEvent> GetActiveEvents(int GameReportId)
        {
            List<VsandSportEvent> oRet = null;

            IEnumerable<VsandSportEvent> oData = oDb.GameReportActiveEvents(GameReportId);

            IEnumerable<VsandGameReportEvent> oGE = (from ge in _context.VsandGameReportEvent.Include(ge => ge.Results)
                                                     where ge.GameReport.GameReportId == GameReportId
                                                     select ge);

            if (oGE != null)
            {
                List<VsandGameReportEvent> oERList = oGE.ToList();
            }

            if (oData != null)
            {
                oRet = oData.ToList();
            }

            return oRet;
        }
        */

        public bool ActivateEvent(int GameReportId, int EventId)
        {
            string sMsg = "";
            return ActivateEvent(GameReportId, EventId, ref sMsg);
        }

        public bool ActivateEvent(int GameReportId, int EventId, ref string sMsg)
        {
            bool bRet = false;
            int iEventId = 0;

            VsandGameReportEvent oEvent = (from e in _context.VsandGameReportEvent
                                           where e.GameReport.GameReportId == GameReportId
                                           && e.SportEvent.SportEventId == EventId
                                           select e).FirstOrDefault();

            if (oEvent == null)
            {
                int iSort = 1;

                VsandGameReportEvent oFirst = (from f in _context.VsandGameReportEvent
                                               where f.GameReport.GameReportId == GameReportId
                                               orderby f.SortOrder descending
                                               select f).FirstOrDefault();

                if (oFirst != null)
                {
                    if (oFirst.SortOrder.HasValue)
                    {
                        iSort = oFirst.SortOrder.Value + 1;
                    }
                    else
                    {
                        int iCount = (from f in _context.VsandGameReportEvent
                                      where f.GameReport.GameReportId == GameReportId
                                      select f).Count();

                        iSort = iCount + 1;
                    }
                }

                oEvent = new VsandGameReportEvent
                {
                    GameReportId = GameReportId,
                    // TODO: EventId or SportEventId ?
                    SportEventId = EventId,
                    SortOrder = iSort
                };

                _context.VsandGameReportEvent.Add(oEvent);

                try
                {
                    _context.SaveChanges();
                    bRet = true;
                    iEventId = oEvent.EventId;
                }
                catch (Exception ex)
                {
                    sMsg = ex.Message;
                    Log.Error(ex, ex.Message);
                }
            }
            else
            {
                iEventId = oEvent.EventId;
                bRet = true;
            }

            if (iEventId > 0)
            {
                // TODO: event remediation
                // VSAND.Events.GameReport.RaiseEventUpdated(new object(), new VSAND.Events.GameReport.GameReportEventEventArgs(iEventId));
            }

            return bRet;
        }

        public bool DeactivateEvent(int GameReportId, int SportEventId)
        {
            string sMsg = "";
            return DeactivateEvent(GameReportId, SportEventId, ref sMsg);
        }

        public bool DeactivateEvent(int GameReportId, int SportEventId, ref string sMsg)
        {
            bool bRet = false;
            int iEventId = 0;

            VsandGameReportEvent oEvent = (from e in _context.VsandGameReportEvent
                                           where e.GameReport.GameReportId == GameReportId
                                           && e.SportEvent.SportEventId == SportEventId
                                           select e).FirstOrDefault();

            if (oEvent != null)
            {
                iEventId = oEvent.EventId;

                _context.VsandGameReportEvent.Remove(oEvent);

                try
                {
                    _context.SaveChanges();
                    bRet = true;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }
            else
            {
                sMsg = "The game report event record could not be found";
            }

            if (iEventId > 0)
            {
                // TODO: event remediation
                // VSAND.Events.GameReport.RaiseEventDeleted(new object(), new VSAND.Events.GameReport.GameReportEventEventArgs(iEventId));
            }

            return bRet;
        }

        public VsandGameReportEvent GetEvent(int GameReportId, int SportEventId)
        {
            VsandGameReportEvent oRet = null;

            oRet = (from e in _context.VsandGameReportEvent
                    where e.GameReport.GameReportId == GameReportId && e.SportEvent.SportEventId == SportEventId
                    select e).FirstOrDefault();

            return oRet;
        }

        public int GetEventId(int GameReportId, int SportEventId)
        {
            int iRet = 0;

            VsandGameReportEvent oEvent = GetEvent(GameReportId, SportEventId);
            if (oEvent != null)
            {
                iRet = oEvent.EventId;
            }

            return iRet;
        }

        public void SortEventsByStartingWeightClass(int GameReportId, bool exhibition)
        {
            int StartMetaId = 0;
            string StartingWC = "";

            // -- Get the starting weight class
            bool bHasChanges = false;

            VsandSportGameMeta oMeta = (from m in _context.VsandSportGameMeta
                                        where m.Sport.SportId == 17 && m.Name == "Starting Weight Class"
                                        select m).FirstOrDefault();

            if (oMeta != null)
            {
                StartMetaId = oMeta.SportGameMetaId;
            }

            VsandGameReportMeta oGameStartMeta = (from gm in _context.VsandGameReportMeta
                                                  where gm.GameReport.GameReportId == GameReportId && gm.SportGameMeta.SportGameMetaId == StartMetaId
                                                  select gm).FirstOrDefault();

            if (oGameStartMeta != null)
            {
                StartingWC = oGameStartMeta.MetaValue;
            }

            if (StartingWC == "999")
            {
                StartingWC = "HWT";
            }

            if (StartingWC == "0")
            {
                StartingWC = "106";
                if (exhibition)
                {
                    StartingWC = "100";
                }
            }

            if (StartingWC != "0")
            {
                // -- We can re-sort th events to the correct starting order
                IEnumerable<VsandGameReportEvent> oGameEvents = (from gre in _context.VsandGameReportEvent
                                                                 where gre.GameReport.GameReportId == GameReportId
                                                                 orderby gre.SortOrder ascending
                                                                 select gre);

                if (oGameEvents != null)
                {
                    VsandSportEvent oRelEvent = (from se in _context.VsandSportEvent
                                                 where se.Sport.SportId == 17 && se.Abbreviation == StartingWC
                                                 select se).FirstOrDefault();

                    if (oRelEvent != null)
                    {
                        int iCurSort = 1;
                        int RelSort = oRelEvent.DefaultSort;

                        IEnumerable<VsandSportEvent> oFirstEvents = (from se in _context.VsandSportEvent
                                                                     where se.Sport.SportId == 17 && se.DefaultSort >= RelSort
                                                                     orderby se.DefaultSort ascending
                                                                     select se);

                        foreach (VsandSportEvent oFirstEvent in oFirstEvents)
                        {
                            int EventId = oFirstEvent.SportEventId;

                            // TODO: sportEventId or eventId
                            VsandGameReportEvent oCurEvent = oGameEvents.FirstOrDefault(ge => ge.SportEventId == EventId);
                            if (oCurEvent != null)
                            {
                                oCurEvent.SortOrder = iCurSort;
                                bHasChanges = true;
                            }
                            iCurSort = iCurSort + 1;
                        }

                        IEnumerable<VsandSportEvent> oNextEvents = (from se in _context.VsandSportEvent
                                                                    where se.Sport.SportId == 17 && se.DefaultSort < RelSort
                                                                    orderby se.DefaultSort ascending
                                                                    select se);

                        foreach (VsandSportEvent oNextEvent in oNextEvents)
                        {
                            int EventId = oNextEvent.SportEventId;

                            // TODO: sportEventId or eventId
                            VsandGameReportEvent oCurEvent = oGameEvents.FirstOrDefault(ge => ge.SportEventId == EventId);
                            if (oCurEvent != null)
                            {
                                oCurEvent.SortOrder = iCurSort;
                                bHasChanges = true;
                            }
                            iCurSort = iCurSort + 1;
                        }
                    }
                }
            }

            if (bHasChanges)
            {
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                }
            }
        }

        public void EnableEventsByPlayFormat(int GameReportId, int SportId)
        {
            string sFormat = "";
            int FormatId = 0;
            int GameReportEventId = 0;
            VsandGameReportEvent oAddEvent = null;

            bool bHasChanges = false;

            VsandGameReport oGR = (from gr in _context.VsandGameReport
                                   where gr.GameReportId == GameReportId
                                   select gr).FirstOrDefault();

            if (oGR != null)
            {
                VsandSportGameMeta oMeta = (from m in _context.VsandSportGameMeta
                                            where m.Sport.SportId == SportId && m.Name == "Format of Play"
                                            select m).FirstOrDefault();

                if (oMeta != null)
                {
                    FormatId = oMeta.SportGameMetaId;
                }
                VsandGameReportMeta oGameFormatMeta = (from gm in _context.VsandGameReportMeta
                                                       where gm.GameReport.GameReportId == GameReportId && gm.SportGameMeta.SportGameMetaId == FormatId
                                                       select gm).FirstOrDefault();

                if (oGameFormatMeta != null)
                {
                    sFormat = oGameFormatMeta.MetaValue;
                }

                int iFormat = 0; // -- 0 is a good default, since it represents Stroke Play, the most typical choice
                int.TryParse(sFormat, out iFormat);

                // TODO: decide waht happens to formatters
                // VSAND.GolfPlayFormat oFormat = new VSAND.GolfPlayFormat();
                // sFormat = oFormat.GetDisplay(iFormat).ToLower;
                VsandSportEvent oEvent = (from se in _context.VsandSportEvent
                                          where se.Name.ToLower().Contains(sFormat) && se.Enabled == true && se.Sport.SportId == SportId
                                          select se).FirstOrDefault();

                if (oEvent != null)
                {
                    oAddEvent = new VsandGameReportEvent
                    {
                        // TODO: SportEventId or EventId
                        SportEventId = oEvent.SportEventId,
                        SortOrder = 1
                    };
                    oGR.Events.Add(oAddEvent);
                    bHasChanges = true;
                }
            }

            if (bHasChanges)
            {
                try
                {
                    _context.SaveChanges();
                    GameReportEventId = oAddEvent.EventId;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex.Message);
                    if (ex.InnerException != null)
                    {
                        Log.Error(ex.InnerException, ex.InnerException.Message);
                    }
                }
            }

            if (GameReportEventId > 0)
            {
                // -- Raise the event that the event was activated in the game
                // TODO: event remediation
                // VSAND.Events.GameReport.RaiseEventUpdated(new object(), new VSAND.Events.GameReport.GameReportEventEventArgs(GameReportEventId));
            }
        }
    }
}
