
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using DEA.DAL.EF;


namespace DEA.DAL.Repository
{
    public static class GuildRepository
    {

        public static async Task<guild> FetchGuildAsync(decimal guildId)
        {
            guild ExistingGuild = await BaseRepository<guild>.SearchFor(c => c.id == guildId).FirstOrDefaultAsync();

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

        public static async Task ModifyAsync(Func<guild, Task> function, decimal guildId)
        {
            var guild = await FetchGuildAsync(guildId);
            await function(guild);
            await BaseRepository<guild>.UpdateAsync(guild);
        }

    }
}