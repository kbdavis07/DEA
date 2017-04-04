using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Models
{
    
    public class Guild
    {

        #region Guild

        public ulong Id { get; set; }
        public List<Mute> Mutes { get; set; }
        public List<Gang> Gangs { get; set; }
        public List<User> Users { get; set; }

        #endregion
        
        #region Roles

        public List<ModRole> ModRoles { get; set; }
        public List<RankRole> RankRoles { get; set; }
        public ulong NsfwRoleId { get; set; }
        public ulong MutedRoleId { get; set; }

        #endregion

        #region Channels

        public ulong ModLogId { get; set; } = 0;
        public ulong DetailedLogsId { get; set; } = 0;
        public ulong GambleId { get; set; } = 0;
        public ulong NsfwId { get; set; } = 0;

        #endregion

        #region Options
        
        [Column("prefix")]
        public string Prefix { get; set; } = "$";
        [Column("nsfw")]
        public bool Nsfw { get; set; } = false;
        [Column("globalchattingmultiplier")]
        public double GlobalChattingMultiplier = 1.0;
        [Column("tempmultiplierincreaserate")]
        public double TempMultiplierIncreaseRate = 0.10;
        [Column("jumprequirement")]
        public double JumpRequirement { get; set; } = 500.0;
        [Column("stealrequirement")]
        public double StealRequirement { get; set; } = 2500.0;
        [Column("robrequirement")]
        public double RobRequirement { get; set; } = 5000.0;
        [Column("bullyrequirement")]
        public double BullyRequirement { get; set; } = 10000.0;
        [Column("fiftyx2requirement")]
        public double FiftyX2Requirement { get; set; } = 25000.0;

        #endregion

        #region Misc
        
        [Column("casenumber")]
        public int CaseNumber { get; set; } = 1;

        #endregion

    }
}
