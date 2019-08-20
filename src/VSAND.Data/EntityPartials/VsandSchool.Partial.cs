namespace VSAND.Data.Entities
{
    public partial class VsandSchool
    {
        public string ListDisplayName
        {
            get {
                string sDesc = this.Name;
                string sCity = "";
                if (this.City != null && !string.IsNullOrEmpty(this.City.Trim()))
                {
                    sCity = this.City.Trim() + ", ";
                }
                string sState = this.State.Trim().ToUpperInvariant();
                if (!string.IsNullOrEmpty(sCity) || !string.IsNullOrEmpty(sState))
                {
                    sDesc = string.Format("{0} ({1}{2})", sDesc, sCity, sState);
                }
                if (!this.Validated)
                {
                    sDesc = "X " + sDesc;
                }
                return sDesc;
            }
        }
    }
}
