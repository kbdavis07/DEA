

using System.Collections.Generic;

namespace DEA.Models
{
   
    public class Guild
    { 
        
        public ulong Id { get; set; }

        //Global
        
        public List<Mute> Mutes { get; set; }
        
        public List<Gang> Gangs { get; set; }
       
        public List<User> Users { get; set; }
		

        //Roles
       
        public List<ModRole> ModRoles { get; set; }
        
        public List<RankRole> RankRoles { get; set; }
        
        public ulong NsfwRoleId { get; set; }
        
        public ulong MutedRoleId { get; set; }

        //Channels
        
        public ulong ModLogId { get; set; } = 0;
        
        public ulong DetailedLogsId { get; set; } = 0;
        
        public ulong GambleId { get; set; } = 0;
        
        public ulong NsfwId { get; set; } = 0;

        //Options
        
        public string Prefix { get; set; } = "$";
        
        public bool Nsfw { get; set; } = false;
       
        public double GlobalChattingMultiplier = 1.0;
        
        public double TempMultiplierIncreaseRate = 0.10;
       
        public double JumpRequirement { get; set; } = 500.0;
        
        public double StealRequirement { get; set; } = 2500.0;
        
        public double RobRequirement { get; set; } = 5000.0;
        
        public double BullyRequirement { get; set; } = 10000.0;
        
        public double FiftyX2Requirement { get; set; } = 25000.0;

        //Misc
        
        public int CaseNumber { get; set; } = 1;
    }
}
