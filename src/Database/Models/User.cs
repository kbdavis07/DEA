using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace DEA.Database.Models
{
    [Table("users")]
    public class User
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

        //Cash system data
        [Column("cash")]
        public double Cash { get; set; } = 0.0;
        [Column("temporarymultiplier")]
        public double TemporaryMultiplier { get; set; } = 1.0;
        [Column("investmentmultiplier")]
        public double InvestmentMultiplier { get; set; } = 1.0;
        [Column("messagecooldown")]
        public TimeSpan MessageCooldown { get; set; } = TimeSpan.FromSeconds(30);

        //Cooldowns
        [Column("message")]
        public DateTimeOffset Message { get; set; } = DateTimeOffset.Now.AddYears(-1);
        [Column("whore")]
        public DateTimeOffset Whore { get; set; } = DateTimeOffset.Now.AddYears(-1);
        [Column("jump")]
        public DateTimeOffset Jump { get; set; } = DateTimeOffset.Now.AddYears(-1);
        [Column("steal")]
        public DateTimeOffset Steal { get; set; } = DateTimeOffset.Now.AddYears(-1);
        [Column("rob")]
        public DateTimeOffset Rob { get; set; } = DateTimeOffset.Now.AddYears(-1);
        [Column("withdraw")]
        public DateTimeOffset Withdraw { get; set; } = DateTimeOffset.Now.AddYears(-1);
    }
}
