using DEA.Services;
using DEA.Database.Repository;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace DEA.Events
{
    class UserEvents
    {
        private DiscordSocketClient _client;

        public UserEvents(DiscordSocketClient client)
        {
            _client = client;
            _client.UserJoined += HandleUserJoin;
            _client.UserBanned += HandleUserBanned;
            _client.UserLeft += HandleUserLeft;
            _client.UserUnbanned += HandleUserUnbanned;
        }

        private async Task HandleUserJoin(SocketGuildUser u)
        {
            await Logger.DetailedLog(u.Guild, "Event", "User Joined", "User", $"{u}", u.Id, new Color(12, 255, 129), false);
            var user = u as IGuildUser;
            var mutedRole = user.Guild.GetRole((ulong)((await GuildRepository.FetchGuildAsync(user.Guild.Id)).MutedRoleId));
            if (mutedRole != null && u.Guild.CurrentUser.GuildPermissions.ManageRoles &&
                mutedRole.Position < u.Guild.CurrentUser.Roles.OrderByDescending(x => x.Position).First().Position)
            {
                await RankHandler.Handle(u.Guild, u.Id);
                if (await MuteRepository.IsMutedAsync(user.Id, user.Guild.Id) && mutedRole != null && user != null) await user.AddRoleAsync(mutedRole);
            }
        }

        private async Task HandleUserBanned(SocketUser u, SocketGuild guild)
        {
            await Logger.DetailedLog(guild, "Action", "Ban", "User", $"{u}", u.Id, new Color(255, 0, 0));
        }

        private async Task HandleUserLeft(SocketGuildUser u)
        {
            await Logger.DetailedLog(u.Guild, "Event", "User Left", "User", $"{u}", u.Id, new Color(255, 114, 14));
        }

        private async Task HandleUserUnbanned(SocketUser u, SocketGuild guild)
        {
            await Logger.DetailedLog(guild, "Action", "Unban", "User", $"<@{u.Id}>", u.Id, new Color(12, 255, 129));
        }
    }
}
