using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Models
{
   
    public class RankRole
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong RoleId { get; set; }
        public double CashRequired { get; set; }
    }
}
