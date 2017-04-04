using DEA.Database.Models;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEA.Database.Repository
{
    public static class UserRepository
    {

        public static async Task ModifyAsync(Func<User, Task> function, SocketCommandContext context)
        {
            var user = await FetchUserAsync(context.User.Id, context.Guild.Id);
            await function(user);
            await BaseRepository<User>.UpdateAsync(user);
        }

        public static async Task ModifyAsync(Func<User, Task> function, ulong userId, ulong guildId)
        {
            var user = await FetchUserAsync(userId, guildId);
            await function(user);
            await BaseRepository<User>.UpdateAsync(user);
        }

        public static async Task<User> FetchUserAsync(SocketCommandContext context)
        {
            var guild = await GuildRepository.FetchGuildAsync(context.Guild.Id);
            var existingUser = guild.Users.FirstOrDefault(x => x.UserId == context.User.Id);
            if (existingUser == null)
            {
                var createdUser = new User()
                {
                    UserId = context.User.Id,
                    GuildId = guild.Id
                };
                guild.Users.Add(createdUser);
                await BaseRepository<Guild>.UpdateAsync(guild);
                return createdUser;
            }
            return existingUser;
        }

        public static async Task<User> FetchUserAsync(ulong userId, ulong guildId)
        {
            var guild = await GuildRepository.FetchGuildAsync(guildId);
            var existingUser = guild.Users.FirstOrDefault(x => x.UserId == userId);
            if (existingUser == null)
            {
                var createdUser = new User()
                {
                    UserId = userId,
                    GuildId = guild.Id
                };
                guild.Users.Add(createdUser);
                await BaseRepository<Guild>.UpdateAsync(guild);
                return createdUser;
            }
            return existingUser;
        }

        public static async Task<double> GetCashAsync(SocketCommandContext context)
        {
            var user = await FetchUserAsync(context.User.Id, context.Guild.Id);
            return user.Cash;
        }

        public static async Task<double> GetCashAsync(ulong userId, ulong guildId)
        {
            var user = await FetchUserAsync(userId, guildId);
            return user.Cash;
        }

        public static async Task EditCashAsync(SocketCommandContext context, double change)
        {
            var user = await FetchUserAsync(context.User.Id, context.Guild.Id);
            user.Cash = Math.Round(user.Cash + change, 2);
            await BaseRepository<User>.UpdateAsync(user);
            await RankHandler.Handle(context.Guild, context.User.Id);
        }

        public static async Task EditCashAsync(SocketCommandContext context, ulong userId, double change)
        {
            var user = await FetchUserAsync(userId, context.Guild.Id);
            user.Cash = Math.Round(user.Cash + change, 2);
            await BaseRepository<User>.UpdateAsync(user);
            await RankHandler.Handle(context.Guild, userId);
        }

        public static async Task<List<User>> AllAsync()
        {
            return await BaseRepository<User>.GetAll().ToListAsync();
        }

        public static async Task<List<User>> AllAsync(ulong guildId)
        {
            return (await GuildRepository.FetchGuildAsync(guildId)).Users;
        }

    }
}