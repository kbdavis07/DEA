namespace DEA.DAL.EF
{
    using System.Data.Entity;

    public partial class DEAContext : DbContext
    {
        public DEAContext()
            : base("name=DEA")
        {
        }

        
        public virtual DbSet<gang> gangs { get; set; }
        public virtual DbSet<guild> guilds { get; set; }
        public virtual DbSet<modrole> modroles { get; set; }
        public virtual DbSet<mute> mutes { get; set; }
        public virtual DbSet<rankrole> rankroles { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<gang>()
                .Property(e => e.guildid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<gang>()
                .Property(e => e.leaderid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<guild>()
                .Property(e => e.id)
                .HasPrecision(20, 0);

            modelBuilder.Entity<guild>()
                .Property(e => e.detailedlogsid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<guild>()
                .Property(e => e.gambleid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<guild>()
                .Property(e => e.modlogid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<guild>()
                .Property(e => e.muteroleid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<guild>()
                .Property(e => e.nsfwid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<guild>()
                .Property(e => e.nsfwroleid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<modrole>()
                .Property(e => e.guildid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<modrole>()
                .Property(e => e.roleid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<mute>()
                .Property(e => e.guildid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<mute>()
                .Property(e => e.userid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<rankrole>()
                .Property(e => e.guildid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<rankrole>()
                .Property(e => e.roleid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<user>()
                .Property(e => e.guildid)
                .HasPrecision(20, 0);

            modelBuilder.Entity<user>()
                .Property(e => e.userid)
                .HasPrecision(20, 0);
        }
    }
}
