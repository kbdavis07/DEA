using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DEA.DAL.EF;
using Discord.Commands;


namespace DEA.DAL.Repository
{
    public static class GangRepository
    {

        public static async Task ModifyAsync(Func<gang, Task> function, Discord.Commands.SocketCommandContext context)
        {
            var gang = await FetchGangAsync(context.User.Id, context.Guild.Id);
            await function(gang);
            await BaseRepository<gang>.UpdateAsync(gang);
        }

        public static async Task ModifyAsync(Func<gang, Task> function, ulong userId, ulong guildId)
        {
            var gang = await FetchGangAsync(userId, guildId);
            await function(gang);
            await BaseRepository<gang>.UpdateAsync(gang);
        }

        public static async Task<gang> FetchGangAsync(SocketCommandContext context)
        {
            var gang = await BaseRepository<gang>.SearchFor(c => (c.leaderid == context.User.Id || c.users.Any(x => x == context.User.Id))
                                       && c.guildid == (decimal)context.Guild.Id).FirstOrDefaultAsync();
            if (gang == null) throw new Exception("This user is not in a gang.");
            return gang;
        }

        public static async Task<gang> FetchGangAsync(ulong userId, ulong guildId)
        {
            var gang = await BaseRepository<gang>.SearchFor(c => (c.leaderid == (decimal)userId || c.users.Any(x => x.id == (decimal)userId)) && c.guildid == (decimal)guildId).FirstOrDefaultAsync();

            if (gang == null) throw new Exception("This user is not in a gang.");
            return gang;
        }

        public static async Task<Gang> FetchGangAsync(string gangName, ulong guildId)
        {
            var gang = await BaseRepository<Gang>.SearchFor(c => c.Name.ToLower() == gangName.ToLower() && c.GuildId == guildId).FirstOrDefaultAsync();
            if (gang == null) throw new Exception("This user is not in a gang.");
            return gang;
        }

        public static async Task<Gang> CreateGangAsync(ulong leaderId, ulong guildId, string name)
        {
            if (await BaseRepository<Gang>.GetAll().AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.GuildId == guildId)) throw new Exception($"There is already a gang by the name {name}.");
            if (name.Length > Config.GANG_NAME_CHAR_LIMIT) throw new Exception($"The length of a gang name may not be longer than {Config.GANG_NAME_CHAR_LIMIT} characters.");
            var CreatedGang = new Gang()
            {
                GuildId = guildId,
                LeaderId = leaderId,
                Name = name
            };
            await BaseRepository<Gang>.InsertAsync(CreatedGang);
            return CreatedGang;
        }

        public static async Task<Gang> DestroyGangAsync(ulong userId, ulong guildId)
        {
            var gang = await FetchGangAsync(userId, guildId);
            await BaseRepository<Gang>.DeleteAsync(gang);
            return gang;
        }

        public static async Task<bool> InGangAsync(ulong userId, ulong guildId)
        {
            return await BaseRepository<Gang>.SearchFor(c => (c.LeaderId == userId || c.Members.Any(x => x == userId)) && c.GuildId == guildId).AnyAsync();
        }

        public static async Task<bool> IsMemberOfAsync(ulong memberId, ulong guildId, ulong userId)
        {
            var gang = await FetchGangAsync(memberId, guildId);
            if (gang.LeaderId == userId || gang.Members.Any(x => x == userId)) return true;
            return false;
        }

        public static async Task<bool> IsFullAsync(ulong userId, ulong guildId)
        {
            var gang = await FetchGangAsync(userId, guildId);
            if (gang.Members.Length == 4) return true;
            return false;
        }

        public static async Task RemoveMemberAsync(ulong memberId, ulong guildId)
        {
            var gang = await FetchGangAsync(memberId, guildId);
            for (int i = 0; i < gang.Members.Length; i++)
                if (gang.Members[i] == memberId) gang.Members[i] = 0;
            await BaseRepository<Gang>.UpdateAsync(gang);
        }

        public static async Task AddMemberAsync(ulong userId, ulong guildId, ulong newMemberId)
        {
            var gang = await FetchGangAsync(userId, guildId);
            for (int i = 0; i < gang.Members.Length; i++)
            {
                if (gang.Members[i] == 0)
                {
                    gang.Members[i] = newMemberId;
                    break;
                }
            }
            await BaseRepository<Gang>.UpdateAsync(gang);
        }

        public static async Task<List<Gang>> AllAsync(ulong guildId)
        {
            return (await BaseRepository<Gang>.GetAll().ToListAsync()).FindAll(x => x.GuildId == guildId);
        }
    }
}