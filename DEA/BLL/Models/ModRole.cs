using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DEA.Models
{
    
    public class ModRole
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong RoleId { get; set; }
        public int PermissionLevel { get; set; }
    }
}
