
using System;

namespace DEA.Models
{
   
    public class User
    {
       
        public int Id { get; set; }
       
        public ulong UserId { get; set; }
       
        //Foreign key back to the guild
        public ulong GuildId { get; set; }

        //Cash system data
       
        public double Cash { get; set; } = 0.0;
        
        public double TemporaryMultiplier { get; set; } = 1.0;
        
        public double InvestmentMultiplier { get; set; } = 1.0;
        
        public TimeSpan MessageCooldown { get; set; } = TimeSpan.FromSeconds(30);

        //Cooldowns
        
        public DateTimeOffset Message { get; set; } = DateTimeOffset.Now.AddYears(-1);
       
        public DateTimeOffset Whore { get; set; } = DateTimeOffset.Now.AddYears(-1);
        
        public DateTimeOffset Jump { get; set; } = DateTimeOffset.Now.AddYears(-1);
       
        public DateTimeOffset Steal { get; set; } = DateTimeOffset.Now.AddYears(-1);
        
        public DateTimeOffset Rob { get; set; } = DateTimeOffset.Now.AddYears(-1);
        
        public DateTimeOffset Withdraw { get; set; } = DateTimeOffset.Now.AddYears(-1);
    }
}
