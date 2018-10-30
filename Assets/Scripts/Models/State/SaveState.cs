using TextSpace.Models.Dtos;
using System;
using System.Collections.Generic;

namespace TextSpace.Models
{
    public class SaveState
    {
        public DateTime SaveTime;
        public ExpeditionDto ExpeditionData;
        public HomeworldDto HomeworldData;
        public List<string> CmdViewCellData;
    }

    public class InvalidSaveState : SaveState { }
}
