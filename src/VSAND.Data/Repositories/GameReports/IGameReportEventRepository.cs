using VSAND.Data.Entities;

namespace VSAND.Data.Repositories
{
    interface IGameReportEventRepository : IRepository<VsandGameReportEvent>
    {
        /*
        List<VsandSportEvent> GetInactiveEvents(int GameReportId);

        List<VsandSportEvent> GetActiveEvents(int GameReportId);
        */

        bool ActivateEvent(int GameReportId, int EventId);

        bool ActivateEvent(int GameReportId, int EventId, ref string sMsg);

        bool DeactivateEvent(int GameReportId, int SportEventId);

        bool DeactivateEvent(int GameReportId, int SportEventId, ref string sMsg);

        VsandGameReportEvent GetEvent(int GameReportId, int SportEventId);

        int GetEventId(int GameReportId, int SportEventId);

        void SortEventsByStartingWeightClass(int GameReportId, bool exhibition);

        void EnableEventsByPlayFormat(int GameReportId, int SportId);
    }
}
