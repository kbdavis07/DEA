namespace DEA.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class modrole
    {
        public int id { get; set; }

        public decimal guildid { get; set; }

        public int permissionlevel { get; set; }

        public decimal roleid { get; set; }

        public virtual guild guild { get; set; }
    }
}
