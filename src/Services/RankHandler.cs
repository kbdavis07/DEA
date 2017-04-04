using DEA.Database.Models;
using DEA.Database.Repository;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DEA
{
    public static class RankHandler
    {
        public static async Task Handle(IGuild guild, ulong userId)
        {
            if (!((await guild.GetCurrentUserAsync()).GuildPermissions.ManageRoles)) return;
            double cash = (await UserRepository.FetchUserAsync(userId, guild.Id)).Cash;
            var user = await guild.GetUserAsync(userId); //FETCHES THE USER
            var currentUser = await guild.GetCurrentUserAsync() as SocketGuildUser; //FETCHES THE BOT'S USER
            var guildData = await GuildRepository.FetchGuildAsync(guild.Id);
            List<IRole> rolesToAdd = new List<IRole>();
            List<IRole> rolesToRemove = new List<IRole>();
            if (guild != null && user != null)
            {
                //CHECKS IF THE ROLE EXISTS AND IF IT IS LOWER THAN THE BOT'S HIGHEST ROLE
                foreach (var rankRole in guildData.RankRoles)
                {
                    var role = guild.GetRole((ulong)rankRole.RoleId);
                    if (role != null && role.Position < currentUser.Roles.OrderByDescending(x => x.Position).First().Position)
                    {
                        if (cash >= rankRole.CashRequired && !user.RoleIds.Any(x => x == rankRole.RoleId)) rolesToAdd.Add(role);
                        if (cash < rankRole.CashRequired && user.RoleIds.Any(x => x == rankRole.RoleId)) rolesToRemove.Add(role);
                    }
                    else
                    {
                        guildData.RankRoles.Remove(rankRole);
                        await BaseRepository<Guild>.UpdateAsync(guildData);
                    }
                }
                if (rolesToAdd.Count >= 1)
                    await user.AddRolesAsync(rolesToAdd);
                else if (rolesToRemove.Count >= 1)
                    await user.RemoveRolesAsync(rolesToRemove);
            }
        }

        public static async Task<IRole> FetchRank(SocketCommandContext context)
        {
            var guild = await GuildRepository.FetchGuildAsync(context.Guild.Id);
            var cash = await UserRepository.GetCashAsync(context);
            IRole role = null;
            foreach (var rankRole in guild.RankRoles.OrderBy(x => x.CashRequired))
                if (cash >= rankRole.CashRequired)
                    role = context.Guild.GetRole((ulong)rankRole.RoleId);
            return role;
        }
    }
}