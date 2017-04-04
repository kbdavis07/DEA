using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Models
{
    
    public class Mute
    {
        public int Id { get; set; }
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }
        public TimeSpan MuteLength { get; set; } 
        public DateTimeOffset MutedAt { get; set; } = DateTimeOffset.Now;
    }
}