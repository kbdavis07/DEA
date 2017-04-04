using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using DEA.Database.Repository;
using System.Linq;
using DEA.Database.Models;

namespace DEA.Modules
{
    [Require(Attributes.Admin)]
    public class Administration : ModuleBase<SocketCommandContext>
    {

        [Command("RoleIDs")]
        [Summary("Gets the ID of all roles in the guild.")]
        [Remarks("RoleIDs")]
        public async Task RoleIDs()
        {
            string message = null;
            foreach (var role in Context.Guild.Roles)
                message += $"{role.Name}: {role.Id}\n";
            var channel = await Context.User.CreateDMChannelAsync();
            await channel.SendMessageAsync(message);
            await ReplyAsync($"{Context.User.Mention}, all Role IDs have been DMed to you!");
        }

        [Command("SetPrefix")]
        [Summary("Sets the guild specific prefix.")]
        [Remarks("SetPrefix <Prefix>")]
        public async Task SetPrefix(string prefix)
        {
            if (prefix.Length > 3) throw new Exception("The maximum character length of a prefix is 3.");
            await GuildRepository.ModifyAsync(x => { x.Prefix = prefix; return Task.CompletedTask; }, Context.Guild.Id);
            await ReplyAsync($"{Context.User.Mention}, You have successfully set the prefix to {prefix}!");
        }

        [Command("AddModRole")]
        [Summary("Adds a moderator role.")]
        [Remarks("AddModRole <@ModRole>")]
        public async Task AddModRole(IRole modRole, int permissionLevel = 1)
        {
            if (permissionLevel < 1 || permissionLevel > 3) throw new Exception("Permission levels:\nModeration: 1\nAdministration: 2\nServer Owner: 3");
            await GuildRepository.ModifyAsync(x => { x.ModRoles.Add(new ModRole()
            {
                RoleId = modRole.Id,
                GuildId = Context.Guild.Id,
                PermissionLevel = permissionLevel
            }); return Task.CompletedTask; }, Context.Guild.Id);
            await ReplyAsync($"{Context.User.Mention}, You have successfully add {modRole.Mention} as a Moderation role with a permission level of {permissionLevel}.");
        }

        [Command("RemoveModRole")]
        [Summary("Removes a moderator role.")]
        [Remarks("RemoveModRole <@ModRole>")]
        public async Task RemoveModRole(IRole modRole)
        {
            var guild = await GuildRepository.FetchGuildAsync(Context.Guild.Id);
            if (!guild.ModRoles.Any(x => x.RoleId == modRole.Id))
                throw new Exception("This role is not a moderator role!");
            await GuildRepository.ModifyAsync(x => { x.ModRoles.Remove(guild.ModRoles.Find(y => y.RoleId == modRole.Id)); return Task.CompletedTask; }, Context.Guild.Id);
            await ReplyAsync($"{Context.User.Mention}, You have successfully set the moderator role to {modRole.Mention}!");
        }

        [Command("SetMutedRole")]
        [Alias("SetMuteRole")]
        [Summary("Sets the muted role.")]
        [Remarks("SetMutedRole <@MutedRole>")]
        public async Task SetMutedRole(IRole mutedRole)
        {
            if (mutedRole.Position >= Context.Guild.CurrentUser.Roles.OrderByDescending(x => x.Position).First().Position)
                throw new Exception($"DEA must be higher in the heigharhy than {mutedRole.Mention}.");
            await GuildRepository.ModifyAsync(x => { x.MutedRoleId = mutedRole.Id; return Task.CompletedTask; }, Context.Guild.Id);
            await ReplyAsync($"{Context.User.Mention}, You have successfully set the muted role to {mutedRole.Mention}!");
        }

        [Command("AddRank")]
        [Summary("Adds a rank role for the DEA cash system.")]
        [Remarks("AddRank <@RankRole> <Cash required>")]
        public async Task AddRank(IRole rankRole, double cashRequired)
        {
            var guild = await GuildRepository.FetchGuildAsync(Context.Guild.Id);
            if (rankRole.Position >= Context.Guild.CurrentUser.Roles.OrderByDescending(x => x.Position).First().Position)
                throw new Exception($"DEA must be higher in the heigharhy than {rankRole.Mention}.");
            if (guild.RankRoles.Any(x => x.RoleId == rankRole.Id))
                throw new Exception("This role is already a rank role.");
            if (guild.RankRoles.Any(x => x.CashRequired == cashRequired))
                throw new Exception("There is already a role set to that amount of cash required.");
            await GuildRepository.ModifyAsync(x => { x.RankRoles.Add(new RankRole()
            {
                RoleId = rankRole.Id,
                CashRequired = cashRequired,
                GuildId = guild.Id
            }); return Task.CompletedTask; }, Context.Guild.Id);
            await ReplyAsync($"You have successfully added the {rankRole.Mention} rank!");
        }

        [Command("RemoveRank")]
        [Summary("Adds a rank role for the DEA cash system.")]
        [Remarks("AddRank <@RankRole> <Cash required>")]
        public async Task RemoveRank(IRole rankRole)
        {
            var guild = await GuildRepository.FetchGuildAsync(Context.Guild.Id);
            if (!guild.RankRoles.Any(x => x.RoleId == rankRole.Id))
                throw new Exception("This role is not a rank role.");
            await GuildRepository.ModifyAsync(x => 
            {
                x.RankRoles.Remove(guild.RankRoles.Find(y => y.RoleId == rankRole.Id));
                return Task.CompletedTask;
            }, Context.Guild.Id);
            await ReplyAsync($"You have successfully added the {rankRole.Mention} rank!");
        }

        [Command("SetModLog")]
        [Summary("Sets the moderation log.")]
        [Remarks("SetModLog <#ModLog>")]
        public async Task SetModLogChannel(ITextChannel modLogChannel)
        {
            await GuildRepository.ModifyAsync(x => { x.ModLogId = modLogChannel.Id; return Task.CompletedTask; }, Context.Guild.Id);
            await ReplyAsync($"{Context.User.Mention}, You have successfully set the moderator log channel to {modLogChannel.Mention}!");
        }

        [Command("SetDetailedLogs")]
        [Summary("Sets the detailed logs.")]
        [Remarks("SetDetailedLogs <#DetailsLogs>")]
        public async Task SetDetailedLogsChannel(ITextChannel detailedLogsChannel)
        {
            await GuildRepository.ModifyAsync(x => { x.DetailedLogsId = detailedLogsChannel.Id; return Task.CompletedTask; }, Context.Guild.Id);
            await ReplyAsync($"{Context.User.Mention}, You have successfully set the detailed logs channel to {detailedLogsChannel.Mention}!");
        }

        [Command("SetGambleChannel")]
        [Alias("SetGamble")]
        [Summary("Sets the gambling channel.")]
        [Remarks("SetGambleChannel <#GambleChannel>")]
        public async Task SetGambleChannel(ITextChannel gambleChannel)
        {
            await GuildRepository.ModifyAsync(x => { x.GambleId = gambleChannel.Id; return Task.CompletedTask; }, Context.Guild.Id);
            await ReplyAsync($"{Context.User.Mention}, You have successfully set the gamble channel to {gambleChannel.Mention}!");
        }

    }
}
