using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DEA.DAL.EF;
using Discord.Commands;


namespace DEA.DAL.Repository
{
    public static class UserRepository
    {

        public static async Task ModifyAsync(Func<user, Task> function, SocketCommandContext context)
        {
            var user = await FetchUserAsync(context.User.Id, context.Guild.Id);
            await function(user);
            await BaseRepository<user>.UpdateAsync(user);
        }

        public static async Task ModifyAsync(Func<user, Task> function, ulong userId, ulong guildId)
        {
            var user = await FetchUserAsync(userId, guildId);
            await function(user);
            await BaseRepository<user>.UpdateAsync(user);
        }

        public static async Task<user> FetchUserAsync(SocketCommandContext context)
        {
            user ExistingUser = await BaseRepository<user>.SearchFor(c => c.userid == context.User.Id && c.guildid == context.Guild.Id).FirstOrDefaultAsync();
            if (ExistingUser == null)
            {
                var CreatedUser = new user()
                {
                    userid = context.User.Id,
                    guildid = context.Guild.Id
                };
                await BaseRepository<user>.InsertAsync(CreatedUser);
                return CreatedUser;
            }
            return ExistingUser;
        }

        public static async Task<user> FetchUserAsync(ulong userId, ulong guildId)
        {
            user ExistingUser = await BaseRepository<user>.SearchFor(c => c.userid == userId && c.guildid == guildId).FirstOrDefaultAsync();
            if (ExistingUser == null)
            {
                var CreatedUser = new user()
                {
                    userid = userId,
                    guildid = guildId
                };
                await BaseRepository<user>.InsertAsync(CreatedUser);
                return CreatedUser;
            }
            return ExistingUser;
        }

        public static async Task<double> GetCashAsync(SocketCommandContext context)
        {
            var user = await FetchUserAsync(context.User.Id, context.Guild.Id);
            return user.cash;
        }

        public static async Task EditCashAsync(SocketCommandContext context, double change)
        {
            var user = await FetchUserAsync(context.User.Id, context.Guild.Id);
            user.cash = Math.Round(user.cash + change, 2);
            await BaseRepository<user>.UpdateAsync(user);
            await RankHandler.Handle(context.Guild, context.User.Id);
        }

        public static async Task EditCashAsync(SocketCommandContext context, ulong userId, double change)
        {
            var user = await FetchUserAsync(userId, context.Guild.Id);
            user.cash = Math.Round(user.cash + change, 2);
            await BaseRepository<user>.UpdateAsync(user);
            await RankHandler.Handle(context.Guild, userId);
        }

        public static async Task<List<user>> AllAsync()
        {
            return await BaseRepository<user>.GetAll().ToListAsync();
        }

        public static async Task<List<user>> AllAsync(ulong guildId)
        {
            return (await BaseRepository<user>.GetAll().ToListAsync()).FindAll(x => x.guildid == guildId);
        }

    }
}