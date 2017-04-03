using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DEA.DAL.Repository;
using Discord;
using Discord.WebSocket;


namespace DEA
{
    public static class RankHandler
    {
        public static async Task Handle(IGuild guild, ulong userId)
        {
            if (!((await guild.GetCurrentUserAsync()).GuildPermissions.ManageRoles)) return;
            double cash = (await UserRepository.FetchUserAsync(userId, guild.Id)).cash;
            var user = await guild.GetUserAsync(userId); //FETCHES THE USER
            var currentUser = await guild.GetCurrentUserAsync() as SocketGuildUser; //FETCHES THE BOT'S USER
            var guildData = await GuildRepository.FetchGuildAsync(guild.Id);
            List<IRole> rolesToAdd = new List<IRole>();
            List<IRole> rolesToRemove = new List<IRole>();
            if (guild != null && user != null)
            {
                //CHECKS IF THE ROLE EXISTS AND IF IT IS LOWER THAN THE BOT'S HIGHEST ROLE
                foreach (var rankRole in guildData.rankroles)
                {
                    //Made some changes here, changed Key --> roleid, value --> cashrequired ? Not sure if this is right or not?

                    var role = guild.GetRole((ulong)rankRole.roleid);
                    if (role != null && role.Position < currentUser.Roles.OrderByDescending(x => x.Position).First().Position)
                    {
                        if (cash >= rankRole.cashrequired && !user.RoleIds.Any(x => x == rankRole.roleid)) rolesToAdd.Add(role); 
                        if (cash < rankRole.cashrequired && user.RoleIds.Any(x => x == rankRole.roleid)) rolesToRemove.Add(role);
                    }
                }
                if (rolesToAdd.Count >= 1)
                    await user.AddRolesAsync(rolesToAdd);
                if (rolesToRemove.Count >= 1)
                    await user.RemoveRolesAsync(rolesToRemove);
            }
        }
    }
}