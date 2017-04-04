using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Models
{
    /// <summary>
    /// Logic Models for the Application, not for use in Database. 
    /// </summary>
    public class Gang
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ulong LeaderId { get; set; }
        public ulong GuildId { get; set; }
        public List<User> Members { get; set; }
        public double Wealth { get; set; } = 0.0;
        public DateTimeOffset Raid { get; set; } = DateTimeOffset.Now.AddYears(-1);
    }
}
