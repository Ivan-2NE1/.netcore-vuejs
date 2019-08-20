using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using VSAND.Data.Identity;

namespace VSAND.Data.Entities
{
    public partial class VsandPowerPointsConfig
    {
        [JsonProperty(PropertyName = "PPConfigId")]
        public int PPConfigId { get; set; }

        public int ScheduleYearId { get; set; }

        public int SportId { get; set; }

        public int IncludeGamesCount { get; set; }

        public bool IncludeTieGames { get; set; }

        [JsonProperty(PropertyName = "BestNGames")]
        public int BestNGames { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? GracePeriodEnd { get; set; }

        public DateTime? SeedingPeriodEnd { get; set; }

        public double WinValue { get; set; }

        public double LossValue { get; set; }

        public double TieValue { get; set; }

        public double GroupWinMultiplier { get; set; }

        public double GroupLossMultiplier { get; set; }

        public double GroupTieMultiplier { get; set; }

        public double ResidualWinMultiplierOppWins { get; set; }

        public double ResidualWinMultiplierOppTies { get; set; }

        public double ResidualWinMultiplierOppLosses { get; set; }

        public double ResidualLossMultiplierOppWins { get; set; }

        public double ResidualLossMultiplierOppLosses { get; set; }

        public double ResidualLossMultiplierOppTies { get; set; }

        public double ResidualTieMultiplierOppWins { get; set; }

        public double ResidualTieMultiplierOppLosses { get; set; }

        public double ResidualTieMultiplierOppTies { get; set; }

        public VsandSport Sport { get; set; }

        public VsandScheduleYear ScheduleYear { get; set; }

        [NotMapped]
        private bool _lockDetermined = false;

        [NotMapped]
        private bool _isLocked = false;

        [NotMapped]
        public bool IsLocked
        {
            get {
                if (!this._lockDetermined)
                {
                    DateTime time = new DateTime(this.EndDate.Year, this.EndDate.Month, this.EndDate.Day, 0x17, 0x3B, 0x3B);
                    if (this.GracePeriodEnd.HasValue)
                        time = this.GracePeriodEnd.Value;
                    if ((DateTime.Compare(time, DateTime.Now) <= 0))
                    {
                        if (this.SeedingPeriodEnd.HasValue)
                        {
                            if ((DateTime.Compare(this.SeedingPeriodEnd.Value, DateTime.Now) >= 0))
                                this._isLocked = true;
                        }
                        else
                            this._isLocked = true;
                    }
                    this._lockDetermined = true;
                }
                return this._isLocked;
            }
        }

        [NotMapped]
        public string IsLockedMsg
        {
            get {
                if (this.SeedingPeriodEnd.HasValue)
                {
                    return string.Concat(new string[] { "<p><b>REMINDER: </b>Changes to game reports that impact your power-points total are prohibited until ", this.SeedingPeriodEnd.Value.ToString("dddd"), ", ", this.SeedingPeriodEnd.Value.ToString("%M/%d"), " at ", this.SeedingPeriodEnd.Value.ToString("%htt").ToLower(), ".</p>" });
                }
                return "<p><b>REMINDER: </b>Changes to game reports that impact your power-points total are prohibited.</p>";
            }
        }

        [NotMapped]
        public bool IsLockedSoon
        {
            get {
                bool flag = false;
                DateTime time = new DateTime(this.EndDate.Year, this.EndDate.Month, this.EndDate.Day, 0x17, 0x3B, 0x3B);
                if (this.GracePeriodEnd.HasValue)
                {
                    time = this.GracePeriodEnd.Value;
                }
                if (((DateTime.Compare(DateTime.Now, time) < 0) && (DateTime.Compare(DateTime.Now.AddDays(5), time) >= 0)))
                {
                    flag = true;
                }
                return flag;
            }
        }

        [NotMapped]
        public bool IsPPEligible
        {
            get {
                bool bEligible = false;
                DateTime time = new DateTime(this.EndDate.Year, this.EndDate.Month, this.EndDate.Day, 23, 59, 59);
                if (this.GracePeriodEnd.HasValue)
                {
                    time = this.GracePeriodEnd.Value;
                }
                if (DateTime.Compare(time, DateTime.Now) >= 0)
                {
                    bEligible = true;
                }
                return bEligible;
            }
        }

        [NotMapped]
        public string LockedSoonMsg
        {
            get {
                DateTime time = new DateTime(this.EndDate.Year, this.EndDate.Month, this.EndDate.Day, 23, 59, 59);
                if (this.GracePeriodEnd.HasValue)
                {
                    time = this.GracePeriodEnd.Value;
                }
                return string.Concat(new string[] { "<p><b>REMINDER: </b>You have until ", time.ToString("%h:mmtt").ToString(), " on ", time.ToString("dddd"), ", ", time.ToString("%M/%d"), ", to finalize your game reports prior to seeding.</p>" });
            }
        }

        public bool IsLockedForUser(IList<ApplicationRole> claims)
        {
            bool isPpLocked = this.IsLocked;

            if (isPpLocked && claims.Any(r => r.Name == "UserFunction.Admin" || r.Name == "UserFunction.GoverningBody"))
            {
                isPpLocked = false;
            }
            return isPpLocked;
        }
    }
}
