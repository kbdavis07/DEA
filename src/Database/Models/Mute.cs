using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Database.Models
{
    [Table("mutes")]
    public class Mute
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("userid")]
        [DataType("decimal(20,0)")]
        public decimal UserId { get; set; }
        [Column("guildid")]
        [DataType("decimal(20,0)")]
        public decimal GuildId { get; set; }
        [Column("mutelength")]
        public TimeSpan MuteLength { get; set; } = Config.DEFAULT_MUTE_TIME;
        [Column("mutedat")]
        public DateTimeOffset MutedAt { get; set; } = DateTimeOffset.Now;
    }
}