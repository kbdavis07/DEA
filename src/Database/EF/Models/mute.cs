namespace DEA.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class mute
    {
        public int id { get; set; }

        public decimal guildid { get; set; }

        public TimeSpan mutelength { get; set; }

        public DateTimeOffset mutedat { get; set; }

        public decimal userid { get; set; }

        public virtual guild guild { get; set; }
    }
}
