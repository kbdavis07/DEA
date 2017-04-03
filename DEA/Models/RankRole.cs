

namespace DEA.Models
{
   
    public class RankRole
    {
       
        public int Id { get; set; }
       
        //Foreign key back to the guild
        public ulong GuildId { get; set; }
       
        public ulong RoleId { get; set; }
        
        public double CashRequired { get; set; }
    }
}
