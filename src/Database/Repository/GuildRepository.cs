using System;
using System.Threading.Tasks;
using DEA.DAL.EF;

namespace DEA.DAL.Repository
{
    public class GuildRepository
    {
        private static DEAContext  db = new DEAContext();

        /// <summary>
        ///Asynchronously finds an Guild entity with the given Primary Key ulong "guildId".
        ///If Guild exists in the context, then it is returned immediately without making a request to the database.
        ///Otherwise, a request is made to the database for a Guild with "guildId" 
        ///and if found, is attached to the context and returned. 
        ///If not found in the context or the database, then a New Guild is created using the "guildId".
        /// </summary>
        /// <param name="guildId">ulong guildId</param>
        /// <returns>Task<guild> Guild Object, either found in database or a new created one.</returns>
        public static async Task<guild> FetchGuildByIdAsync(ulong guildId)
        {
            guild ExistingGuild = await db.guilds.FindAsync((decimal)guildId); 
            
            if (ExistingGuild == null)
            {
                var CreatedGuild = new guild()
                {
                    id = guildId
                };
                await BaseRepository<guild>.InsertAsync(CreatedGuild);

                return CreatedGuild;
            }
            
            return ExistingGuild;
        }
        
        public static async Task ModifyAsync(Func<guild, Task> function, ulong guildId)
        {
            var guild = await FetchGuildByIdAsync(guildId);
            await function(guild);
            await BaseRepository<guild>.UpdateAsync(guild);
        }

    }
}