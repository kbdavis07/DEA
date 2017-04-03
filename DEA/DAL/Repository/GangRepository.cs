using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DEA.DAL.EF;
using Discord.Commands;


namespace DEA.DAL.Repository
{
    public static class GangRepository
    {

        public static async Task ModifyAsync(Func<gang, Task> function, SocketCommandContext context)
        {
            var gang = await FetchGangAsync(context.User.Id, context.Guild.Id);
            await function(gang);
            await BaseRepository<gang>.UpdateAsync(gang);
        }

        public static async Task ModifyAsync(Func<gang, Task> function, ulong userid, ulong guildId)
        {
            var gang = await FetchGangAsync(userid, guildId);
            await function(gang);
            await BaseRepository<gang>.UpdateAsync(gang);
        }

        public static async Task<gang> FetchGangAsync(SocketCommandContext context)
        {
            var gang = await BaseRepository<gang>.SearchFor(c => (c.leaderid == context.User.Id || c.users.Any(x => x.id == (decimal)context.User.Id))
                                       && c.guildid == (decimal)context.Guild.Id).FirstOrDefaultAsync();
            if (gang == null) throw new Exception("This user is not in a gang.");
            return gang;
        }

        public static async Task<gang> FetchGangAsync(ulong userid, ulong guildId)
        {
            var gang = await BaseRepository<gang>.SearchFor(c => (c.leaderid == (decimal)userid || c.users.Any(x => x.id == (decimal)userid)) && c.guildid == (decimal)guildId).FirstOrDefaultAsync();

            if (gang == null) throw new Exception("This user is not in a gang.");
            return gang;
        }

        public static async Task<gang> FetchGangAsync(string gangName, ulong guildId)
        {
            var gang = await BaseRepository<gang>.SearchFor(c => c.name.ToLower() == gangName.ToLower() && c.guildid == (decimal)guildId).FirstOrDefaultAsync();
            if (gang == null) throw new Exception("This user is not in a gang.");
            return gang;
        }

        public static async Task<gang> CreateGangAsync(ulong leaderId, ulong guildId, string name)
        {
            if (await BaseRepository<gang>.GetAll().AnyAsync(x => x.name.ToLower() == name.ToLower() && x.guildid == (decimal)guildId))
            {
                throw new Exception($"There is already a gang by the name {name}.");
            }
               

            //if (name.Length > Config.GANG_NAME_CHAR_LIMIT)
            //{
            //    throw new Exception($"The length of a gang name may not be longer than {Config.GANG_NAME_CHAR_LIMIT} characters.");
            //}

                
            var CreatedGang = new gang()
            {
                guildid = (decimal)guildId,
                leaderid = (decimal)leaderId,
                name = name
            };
            await BaseRepository<gang>.InsertAsync(CreatedGang);
            return CreatedGang;
        }

        public static async Task<gang> DestroyGangAsync(ulong userid, ulong guildId)
        {
            var gang = await FetchGangAsync(userid, guildId);
            await BaseRepository<gang>.DeleteAsync(gang);
            return gang;
        }

        public static async Task<bool> InGangAsync(ulong userid, ulong guildId)
        {
            return await BaseRepository<gang>.SearchFor(c => (c.leaderid == (decimal)userid || c.users.Any(x => x.id == (decimal)userid)) && c.guildid == (decimal)guildId).AnyAsync();
        }

        public static async Task<bool> IsMemberOfAsync(ulong memberId, ulong guildId, ulong userid)
        {
            var gang = await FetchGangAsync(memberId, guildId);
            if (gang.leaderid == (decimal)userid || gang.users.Any(x => x.id == (decimal)userid)) return true;
            return false;
        }

        //public static async Task<bool> IsFullAsync(ulong userid, ulong guildId)
        //{
        //    var gang = await FetchGangAsync(userid, guildId);

        //    if (gang.users.Length == 4) return true;
        //    return false;
        //}

        //public static async Task RemoveMemberAsync(ulong memberId, ulong guildId)
        //{
        //    var gang = await FetchGangAsync(memberId, guildId);

        //    for (int i = 0; i < gang.users.Length; i++)
        //        if (gang.Members[i] == memberId) gang.Members[i] = 0;
        //    await BaseRepository<Gang>.UpdateAsync(gang);
        //}

        //public static async Task AddMemberAsync(ulong userid, ulong guildId, ulong newMemberId)
        //{
        //    var gang = await FetchGangAsync(userid, guildId);
        //    for (int i = 0; i < gang.Members.Length; i++)
        //    {
        //        if (gang.Members[i] == 0)
        //        {
        //            gang.Members[i] = newMemberId;
        //            break;
        //        }
        //    }
        //    await BaseRepository<gang>.UpdateAsync(gang);
        //}

        public static async Task<List<gang>> AllAsync(ulong guildId)
        {
            return (await BaseRepository<gang>.GetAll().ToListAsync()).FindAll(x => x.guildid == (decimal)guildId);
        }
    }
}