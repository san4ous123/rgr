using System;
using System.Collections.Generic;

namespace VisualDataBase
{
    public partial class PlayersSeason
    {
        public long IdPlayer { get; set; }
        public long IdSeason { get; set; }
        public long? IdNation { get; set; }
        public long? Age { get; set; }
        public char? Position { get; set; }
        public long? GamesPlayed { get; set; }
        public long? GoalsScored { get; set; }
        public long? Assists { get; set; }
        public long? Points { get; set; }

        public virtual Nation? IdNationNavigation { get; set; }
    }
}
