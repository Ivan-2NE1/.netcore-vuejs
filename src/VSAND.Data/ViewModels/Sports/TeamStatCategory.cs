using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class TeamStatCategory : StatCategoryBase<TeamStat>
    {
        public int SportTeamStatCategoryId { get; }
        public string Name { get; }
        public List<TeamStat> Stats { get; }

        public TeamStatCategory()
        {

        }

        public TeamStatCategory(VsandSportTeamStatCategory cat)
        {
            this.SportTeamStatCategoryId = cat.SportTeamStatCategoryId;
            this.Name = cat.Name;
            this.Stats = (from s in cat.TeamStats where s.Enabled orderby s.SortOrder ascending select new TeamStat(s)).ToList();
        }
    }
}
