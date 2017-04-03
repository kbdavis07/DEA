namespace DEA.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user
    {
        public int id { get; set; }

        public double cash { get; set; }

        public int? GangId { get; set; }

        public decimal guildid { get; set; }

        public double investmentmultiplier { get; set; }

        public DateTimeOffset jump { get; set; }

        public DateTimeOffset message { get; set; }

        public TimeSpan messagecooldown { get; set; }

        public DateTimeOffset rob { get; set; }

        public DateTimeOffset steal { get; set; }

        public double temporarymultiplier { get; set; }

        public decimal userid { get; set; }

        public DateTimeOffset whore { get; set; }

        public DateTimeOffset withdraw { get; set; }

        public virtual gang gang { get; set; }

        public virtual guild guild { get; set; }
    }
}
