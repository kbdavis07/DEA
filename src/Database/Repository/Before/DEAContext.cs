using Microsoft.EntityFrameworkCore;
using DEA.Database.Models;

namespace DEA.Database.Repository
{
    public class DEAContext : DbContext
    {
        public DbSet<Gang> Gangs { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<ModRole> ModRoles { get; set; }
        public DbSet<Mute> Mutes { get; set; }
        public DbSet<RankRole> RankRoles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source = (LocalDb)\MSSQLLocalDB; Initial Catalog = DEA; Integrated Security = True; Pooling = False");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gang>().Property(x => x.LeaderId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<Gang>().Property(x => x.GuildId).HasColumnType("decimal(20,0)").IsRequired(true);

            modelBuilder.Entity<Guild>().Property(x => x.Id).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<Guild>().Property(x => x.NsfwRoleId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<Guild>().Property(x => x.MutedRoleId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<Guild>().Property(x => x.ModLogId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<Guild>().Property(x => x.DetailedLogsId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<Guild>().Property(x => x.GambleId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<Guild>().Property(x => x.NsfwId).HasColumnType("decimal(20,0)").IsRequired(true);

            modelBuilder.Entity<ModRole>().Property(x => x.GuildId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<ModRole>().Property(x => x.RoleId).HasColumnType("decimal(20,0)").IsRequired(true);

            modelBuilder.Entity<Mute>().Property(x => x.UserId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<Mute>().Property(x => x.GuildId).HasColumnType("decimal(20,0)").IsRequired(true);

            modelBuilder.Entity<RankRole>().Property(x => x.GuildId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<RankRole>().Property(x => x.RoleId).HasColumnType("decimal(20,0)").IsRequired(true);

            modelBuilder.Entity<User>().Property(x => x.UserId).HasColumnType("decimal(20,0)").IsRequired(true);
            modelBuilder.Entity<User>().Property(x => x.GuildId).HasColumnType("decimal(20,0)").IsRequired(true);

        }


    }
}