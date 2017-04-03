using System;
using System.Data.Entity;
using System.Threading.Tasks;
using DEA.DAL.EF;


namespace DEA.DAL.Repository
{
    public static class MuteRepository
    {

        public static async Task AddMuteAsync(ulong UserID, ulong GuildID, TimeSpan muteLength)
        {
            await BaseRepository<mute>.InsertAsync(new mute()
            {
                userid = UserID,
                guildid = GuildID,
                mutelength = muteLength
            });
        }

        public static async Task<bool> IsMutedAsync(ulong UserID, ulong GuildID)
        {
            return await BaseRepository<mute>.SearchFor(c => c.userid == UserID && c.guildid == GuildID).AnyAsync();
        }

        public static async Task RemoveMuteAsync(ulong UserID, ulong GuildID)
        {
            var muted = await BaseRepository<mute>.SearchFor(c => c.userid == UserID && c.guildid == GuildID).FirstOrDefaultAsync();
            if (muted != null) await BaseRepository<mute>.DeleteAsync(muted);
        }

    }
}