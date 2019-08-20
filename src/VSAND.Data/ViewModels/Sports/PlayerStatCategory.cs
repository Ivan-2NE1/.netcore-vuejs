using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;
using System.Linq;

namespace VSAND.Data.ViewModels.Sports
{
    public class PlayerStatCategory : StatCategoryBase<PlayerStat>
    {
        public int SportPlayerStatCategoryId { get; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public List<PlayerStat> Stats { get; set; }
        public PlayerStatCategory()
        {

        }

        public PlayerStatCategory(VsandSportPlayerStatCategory cat)
        {
            this.SportPlayerStatCategoryId = cat.SportPlayerStatCategoryId;
            this.Name = cat.Name;
            this.SortOrder = cat.SortOrder;
            this.Stats = (from s in cat.PlayerStats where s.Enabled orderby s.SortOrder ascending select new PlayerStat(s)).ToList();
        }
    }
}
