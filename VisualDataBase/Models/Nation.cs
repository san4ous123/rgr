using System;
using System.Collections.Generic;

namespace VisualDataBase
{
    public partial class Nation
    {
        public Nation()
        {
            PlayersSeasons = new HashSet<PlayersSeason>();
        }

        public long Id { get; set; }
        public string Title { get; set; } = null!;

        public virtual ICollection<PlayersSeason> PlayersSeasons { get; set; }
    }
}
