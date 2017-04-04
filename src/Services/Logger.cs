using DEA.Database.Repository;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace DEA.Services
{
    public static class Logger
    {
        public static async Task ModLog(SocketCommandContext context, string action, Color color, string reason, IUser subject = null, string extra = "")
        {
            var guild = await GuildRepository.FetchGuildAsync(context.Guild.Id);
            EmbedFooterBuilder footer = new EmbedFooterBuilder()
            {
                IconUrl = "http://i.imgur.com/BQZJAqT.png",
                Text = $"Case #{guild.CaseNumber}"
            };
            EmbedAuthorBuilder author = new EmbedAuthorBuilder()
            {
                IconUrl = context.User.GetAvatarUrl(),
                Name = $"{context.User.Username}#{context.User.Discriminator}"
            };

            string userText = null;
            if (subject != null) userText = $"\n** User:** { subject} ({ subject.Id})";
            var builder = new EmbedBuilder()
            {
                Author = author,
                Color = color,
                Description = $"**Action:** {action}{extra}{userText}\n**Reason:** {reason}",
                Footer = footer
            }.WithCurrentTimestamp();

            if (context.Guild.GetTextChannel((ulong)guild.ModLogId) != null)
            {
                await context.Guild.GetTextChannel((ulong)guild.ModLogId).SendMessageAsync("", embed: builder);
                await GuildRepository.ModifyAsync(x => { x.CaseNumber++; return Task.CompletedTask; }, context.Guild.Id);
            }
        }

        public static async Task DetailedLog(SocketGuild guild, string actionType, string action, string objectType, string objectName, ulong id, Color color, bool incrementCaseNumber = true)
        {
            var guildData = await GuildRepository.FetchGuildAsync(guild.Id);
            if (guild.GetTextChannel((ulong)guildData.DetailedLogsId) != null)
            {
                var channel = guild.GetTextChannel((ulong)guildData.DetailedLogsId);
                if (guild.CurrentUser.GuildPermissions.EmbedLinks && (guild.CurrentUser as IGuildUser).GetPermissions(channel as SocketTextChannel).SendMessages
                    && (guild.CurrentUser as IGuildUser).GetPermissions(channel as SocketTextChannel).EmbedLinks)
                {
                    string caseText = $"Case #{guildData.CaseNumber}";
                    if (!incrementCaseNumber) caseText = id.ToString();
                    EmbedFooterBuilder footer = new EmbedFooterBuilder()
                    {
                        IconUrl = "http://i.imgur.com/BQZJAqT.png",
                        Text = caseText
                    };

                    string idText = null;
                    if (incrementCaseNumber) idText = $"\n**Id:** {id}";
                    var builder = new EmbedBuilder()
                    {
                        Color = color,
                        Description = $"**{actionType}:** {action}\n**{objectType}:** {objectName}{idText}",
                        Footer = footer
                    }.WithCurrentTimestamp();

                    await guild.GetTextChannel((ulong)guildData.DetailedLogsId).SendMessageAsync("", embed: builder);
                    if (incrementCaseNumber) await GuildRepository.ModifyAsync(x => { x.CaseNumber++; return Task.CompletedTask; }, guild.Id);
                }
            }
        }

        public static async Task Cooldown(SocketCommandContext context, string command, TimeSpan timeSpan)
        {
            var builder = new EmbedBuilder()
            {
                Title = $"{command} cooldown for {context.User}",
                Description = $"{timeSpan.Hours} Hours\n{timeSpan.Minutes} Minutes\n{timeSpan.Seconds} Seconds",
                Color = new Color(49, 62, 255)
            };
            await context.Channel.SendMessageAsync("", embed: builder);
        }
    }
}
