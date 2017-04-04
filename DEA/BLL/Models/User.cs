using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace DEA.Models
{
    [Table("users")]
    public class User
    {
        #region User

        public int Id { get; set; }
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }

        #endregion

        #region Cash System Data

        public double Cash { get; set; } = 0.0;
        public double TemporaryMultiplier { get; set; } = 1.0;
        public double InvestmentMultiplier { get; set; } = 1.0;
        public TimeSpan MessageCooldown { get; set; } = TimeSpan.FromSeconds(30);

        #endregion

        #region Cooldowns

        public DateTimeOffset Message { get; set; } = DateTimeOffset.Now.AddYears(-1);
        public DateTimeOffset Whore { get; set; } = DateTimeOffset.Now.AddYears(-1);
        public DateTimeOffset Jump { get; set; } = DateTimeOffset.Now.AddYears(-1);
        public DateTimeOffset Steal { get; set; } = DateTimeOffset.Now.AddYears(-1);
        public DateTimeOffset Rob { get; set; } = DateTimeOffset.Now.AddYears(-1);
        public DateTimeOffset Withdraw { get; set; } = DateTimeOffset.Now.AddYears(-1);
        
        #endregion
    }
}
