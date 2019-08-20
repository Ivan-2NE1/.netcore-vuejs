using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Teams
{
    public class TeamRecord
    {
        public int Wins { get; }
        public int Losses { get; }
        public int Ties { get; }

        public bool HasData
        {
            get
            {
                return Wins + Losses + Ties > 0;
            }
        }

        public TeamRecord()
        {

        }

        public TeamRecord(int wins, int losses, int ties)
        {
            Wins = wins;
            Losses = losses;
            Ties = ties;
        }

        public TeamRecord(TeamCustomCode wins, TeamCustomCode losses, TeamCustomCode ties)
        {
            if (wins != null)
            {
                Wins = wins.GetValue<int>();
            }
            if (losses != null)
            {
                Losses = losses.GetValue<int>();
            }
            if (ties != null)
            {
                Ties = ties.GetValue<int>();
            }
        }

        public TeamRecord(VsandTeamCustomCode wins, VsandTeamCustomCode losses, VsandTeamCustomCode ties)
        {
            if (wins != null)
            {
                Wins = wins.GetValue<int>();
            }
            if (losses != null)
            {
                Losses = losses.GetValue<int>();
            }
            if (ties != null)
            {
                Ties = ties.GetValue<int>();
            }
        }

        public string Format()
        {
            string record = $"{Wins}-{Losses}";
            if (Ties > 0)
            {
                record = $"{record}-{Ties}";
            }
            return record;
        }
    }
}
