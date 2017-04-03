
using System;
using System.Collections.Generic;

namespace DEA.Models
{
   
    public class Gang
    {
        public int Id { get; set; }
       
        public string Name { get; set; }
       
        public ulong LeaderId { get; set; }
       
        public ulong GuildId { get; set; }
        //foreign key back to guild
        
        public List<User> Members { get; set; }
        
        public double Wealth { get; set; } = 0.0;
        
        public DateTimeOffset Raid { get; set; } = DateTimeOffset.Now.AddYears(-1);
    }
}
