namespace DEA.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class gang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public gang()
        {
            users = new HashSet<user>();
        }

        public int id { get; set; }

        public decimal guildid { get; set; }

        public decimal leaderid { get; set; }

        public string name { get; set; }

        public DateTimeOffset raid { get; set; }

        public double wealth { get; set; }

        public virtual guild guild { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> users { get; set; }
    }
}
