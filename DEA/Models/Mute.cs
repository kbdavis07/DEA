

using System;

namespace DEA.Models
{
   
    public class Mute
    {
       
        public int Id { get; set; }
        
        public ulong userid { get; set; }
       
        //Foreign key back to the guild
        public ulong GuildId { get; set; }
        
        public TimeSpan MuteLength { get; set; } /*= Config.DEFAULT_MUTE_TIME;*/
        
        public DateTimeOffset MutedAt { get; set; } = DateTimeOffset.Now;
    }
}