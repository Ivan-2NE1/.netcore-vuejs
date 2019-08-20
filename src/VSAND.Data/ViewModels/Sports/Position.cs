using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class Position
    {
        public int SportPositionId { get; set; }

        [MaxLength(50)]
        [DisplayName("Name")]
        public string Name { get; set; }

        [MaxLength(10)]
        [DisplayName("Abbreviation")]
        public string Abbreviation { get; set; }

        [DisplayName("Sort Order")]
        public int SortOrder { get; set; }

        public int SportId { get; set; }

        public List<int> FeaturedStatIds;

        public Position()
        {

        }

        public Position(VsandSportPosition oPosition)
        {
            this.SportPositionId = oPosition.SportPositionId;
            this.Name = oPosition.Name;
            this.Abbreviation = oPosition.Abbreviation;
            this.SortOrder = oPosition.SortOrder.HasValue ? oPosition.SortOrder.Value : 0;
            this.SportId = oPosition.SportId;
            if (oPosition.FeaturedStatIds != null && oPosition.FeaturedStatIds.Trim() != "")
            {
                this.FeaturedStatIds = oPosition.FeaturedStatIds.Split(',').Select(s => int.Parse(s.Trim())).ToList();
            }
        }

        public void SetSport(int sportId)
        {
            this.SportId = sportId;
        }

        public VsandSportPosition ToEntity()
        {
            var oRet = new VsandSportPosition()
            {
                SportPositionId = this.SportPositionId,
                Name = this.Name,
                Abbreviation = this.Abbreviation,
                SortOrder = this.SortOrder,
                SportId = this.SportId
            };
            return oRet;
        }
    }
}
