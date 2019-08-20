using System;
using System.Collections.Generic;
using System.Text;

namespace VSAND.Data.ViewModels.GameReport
{
    /// <summary>
    /// Right now this class is aspirational for use on the Team dashboard, but i am not sure it will work
    /// the way I want it to, because that also tracks the team's record game over game. We'll see.
    /// My vision is that this is attached to the GameReportSummary (optionally) when invoked with a reference teamid
    /// </summary>
    public class Outcome
    {
        public string result { get; }
    }
}
