using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Database.Models
{
    [Table("guilds")]
    public class Guild
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        [DataType("decimal(20,0)")]
        public decimal Id { get; set; }

        //Global
        [Column("mutes")]
        public List<Mute> Mutes { get; set; }
        [Column("gangs")]
        public List<Gang> Gangs { get; set; }
        [Column("users")]
        public List<User> Users { get; set; }
		

        //Roles
        [Column("modroles")]
        public List<ModRole> ModRoles { get; set; }
        [Column("rankroles")]
        public List<RankRole> RankRoles { get; set; }
        [Column("nsfwroleid")]
        [DataType("decimal(20,0)")]
        public decimal NsfwRoleId { get; set; }
        [Column("muteroleid")]
        [DataType("decimal(20,0)")]
        public decimal MutedRoleId { get; set; }

        //Channels
        [Column("modlogid")]
        [DataType("decimal(20,0)")]
        public decimal ModLogId { get; set; } = 0;
        [Column("detailedlogsid")]
        [DataType("decimal(20,0)")]
        public decimal DetailedLogsId { get; set; } = 0;
        [Column("gambleid")]
        [DataType("decimal(20,0)")]
        public decimal GambleId { get; set; } = 0;
        [Column("nsfwid")]
        [DataType("decimal(20,0)")]
        public decimal NsfwId { get; set; } = 0;

        //Options
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

        //Misc
        [Column("casenumber")]
        public int CaseNumber { get; set; } = 1;
    }
}
