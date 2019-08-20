namespace VSAND.Data.ViewModels
{
    public class TeamListItem
    {
        public int TeamId { get; set; } = 0;
        public string Name { get; set; } = "";
        public string ActualName { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";

        public TeamListItem(int iTeamId, string sName, string sActualName, string sCity, string sState)
        {
            this.TeamId = iTeamId;
            this.Name = sName;
            this.ActualName = sActualName;
            this.City = sCity;
            this.State = sState;
        }
    }
}
