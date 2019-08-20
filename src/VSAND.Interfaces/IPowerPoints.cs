namespace VSAND.Interfaces
{
    public interface IPowerPoints
    {
        int IncludeGames { get; set; }
        int EligibleGamesCount { get; set; }
        double PowerPoints { get; set; }
        double TieBreak();
    }
}
