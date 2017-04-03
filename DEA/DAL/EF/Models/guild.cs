namespace DEA.DAL.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class guild
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public guild()
        {
            gangs = new HashSet<gang>();
            modroles = new HashSet<modrole>();
            mutes = new HashSet<mute>();
            rankroles = new HashSet<rankrole>();
            users = new HashSet<user>();
        }

        public decimal id { get; set; }

        public double bullyrequirement { get; set; }

        public int casenumber { get; set; }

        public decimal detailedlogsid { get; set; }

        public double fiftyx2requirement { get; set; }

        public decimal gambleid { get; set; }

        public double jumprequirement { get; set; }

        public decimal modlogid { get; set; }

        public decimal? muteroleid { get; set; }

        public bool nsfw { get; set; }

        public decimal nsfwid { get; set; }

        public decimal? nsfwroleid { get; set; }

        public string prefix { get; set; }

        public double robrequirement { get; set; }

        public double stealrequirement { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<gang> gangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<modrole> modroles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mute> mutes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rankrole> rankroles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> users { get; set; }
    }
}
