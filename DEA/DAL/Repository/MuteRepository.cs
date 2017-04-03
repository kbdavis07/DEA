using System;
using System.Data.Entity;
using System.Threading.Tasks;
using DEA.DAL.EF;


namespace DEA.DAL.Repository
{
    public static class MuteRepository
    {

        public static async Task AddMuteAsync(ulong userid, ulong GuildID, TimeSpan muteLength)
        {
            await BaseRepository<mute>.InsertAsync(new mute()
            {
                userid = userid,
                guildid = GuildID,
                mutelength = muteLength
            });
        }

        public static async Task<bool> IsMutedAsync(ulong userid, ulong GuildID)
        {
            return await BaseRepository<mute>.SearchFor(c => c.userid == userid && c.guildid == GuildID).AnyAsync();
        }

        public static async Task RemoveMuteAsync(ulong userid, ulong GuildID)
        {
            var muted = await BaseRepository<mute>.SearchFor(c => c.userid == userid && c.guildid == GuildID).FirstOrDefaultAsync();
            if (muted != null) await BaseRepository<mute>.DeleteAsync(muted);
        }

    }
}