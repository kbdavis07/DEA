

namespace DEA.Models
{
    
    public class ModRole
    {
        
        public int Id { get; set; }
        
        //Foreign key back to the guild
        public ulong GuildId { get; set; }
        
        public ulong RoleId { get; set; }
        
        public int PermissionLevel { get; set; }
    }
}
