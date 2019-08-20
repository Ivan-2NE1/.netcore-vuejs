using System;
using System.Collections.Generic;
using System.Text;
using VSAND.Data.Entities;

namespace VSAND.Data.ViewModels.Sports
{
    public class GameMeta
    {
        public int sportGameMetaId { get; }
        public string name { get; }
        public int sortOrder { get; }
        public string valueType { get; }
        public string promptHelp { get; }

        public GameMeta()
        {

        }

        public GameMeta(VsandSportGameMeta oMeta)
        {
            this.sportGameMetaId = oMeta.SportGameMetaId;
            this.name = oMeta.Name;
            this.sortOrder = oMeta.SortOrder;
            this.valueType = oMeta.ValueType;
            this.promptHelp = oMeta.PromptHelp;
        }
    }
}
