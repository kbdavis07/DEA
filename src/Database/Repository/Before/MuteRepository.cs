using DEA.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DEA.Database.Repository
{
    public static class MuteRepository 
    {

        public static async Task AddMuteAsync(ulong userId, ulong guildId, TimeSpan muteLength)
        {
            var guild = await GuildRepository.FetchGuildAsync(guildId);
            guild.Mutes.Add(new Mute()
            {
                UserId = userId,
                GuildId = guild.Id,
                MuteLength = muteLength
            });
            await BaseRepository<Guild>.UpdateAsync(guild);
        }

        public static async Task<bool> IsMutedAsync(ulong userId, ulong guildId)
        {
            return await BaseRepository<Mute>.SearchFor(c => c.UserId == userId && c.GuildId == guildId).AnyAsync();
        }

        public static async Task RemoveMuteAsync(ulong userId, ulong guildId)
        {
            var muted = await BaseRepository<Mute>.SearchFor(c => c.UserId == userId && c.GuildId == guildId).FirstOrDefaultAsync();
            if (muted != null) await BaseRepository<Mute>.DeleteAsync(muted);
        }

    }
}