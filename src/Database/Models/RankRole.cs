using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Database.Models
{
    [Table("rankroles")]
    public class RankRole
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("guildid")]
        [DataType("decimal(20,0)")]
        public decimal GuildId { get; set; }
        [Column("roleid")]
        [DataType("decimal(20,0)")]
        public decimal RoleId { get; set; }
        [Column("cashrequired")]
        public double CashRequired { get; set; }
    }
}
