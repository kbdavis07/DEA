using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Database.Models
{
    [Table("gangs")]
    public class Gang
    {
        [Key]
        [Column("id")]
        [DataType("decimal(20,0)")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("leaderid")]
        [DataType("decimal(20,0)")]
        public decimal LeaderId { get; set; }
        [Column("guildid")]
        [DataType("decimal(20,0)")]
        public decimal GuildId { get; set; }
        //foreign key back to guild
        [Column("members")]
        public List<User> Members { get; set; }
        [Column("wealth")]
        public double Wealth { get; set; } = 0.0;
        [Column("raid")]
        public DateTimeOffset Raid { get; set; } = DateTimeOffset.Now.AddYears(-1);
    }
}
