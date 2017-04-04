using DEA.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DEA.Database.Repository
{
    public static class GuildRepository
    {

        public static async Task<Guild> FetchGuildAsync(ulong guildId)
        {
            Guild ExistingGuild = await BaseRepository<Guild>.SearchFor(c => c.Id == guildId).FirstOrDefaultAsync();
            if (ExistingGuild == null)
            {
                var CreatedGuild = new Guild()
                {
                    Id = guildId
                };
                await BaseRepository<Guild>.InsertAsync(CreatedGuild);
                return CreatedGuild;
            }
            return ExistingGuild;
        }

        public static async Task ModifyAsync(Func<Guild, Task> function, ulong guildId)
        {
            var guild = await FetchGuildAsync(guildId);
            await function(guild);
            await BaseRepository<Guild>.UpdateAsync(guild);
        }

    }
}