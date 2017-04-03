using Discord.WebSocket;
using System;
using System.Timers;
using DEA.SQLite.Repository;
using DEA.SQLite.Models;
using System.Linq;
using Discord;
using System.Threading.Tasks;

namespace DEA.Services
{
    public class RecurringFunctions
    {

        private DiscordSocketClient _client;

        public RecurringFunctions(DiscordSocketClient client)
        {
            _client = client;
            ResetTemporaryMultiplier();
            AutoUnmute();
            ApplyInterestRate();
        }

        private void ResetTemporaryMultiplier()
        {
            Timer t = new Timer(TimeSpan.FromHours(1).TotalMilliseconds);
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(OnTimedTempMultiplierReset);
            t.Start();
        }

        private async void OnTimedTempMultiplierReset(object source, ElapsedEventArgs e)
        {
            foreach (User user in BaseRepository<User>.GetAll())
                if (user.temporarymultiplier != 1) user.temporarymultiplier = 1;

            using (var db = new DbContext())
            {
                await db.SaveChangesAsync();
            }
        }

        private void ApplyInterestRate()
        {
            Timer t = new Timer(TimeSpan.FromHours(1).TotalMilliseconds);
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(OnTimedApplyInterest);
            t.Start();
        }

        private async void OnTimedApplyInterest(object source, ElapsedEventArgs e)
        {
            foreach (var gang in BaseRepository<Gang>.GetAll())
            {
                var InterestRate = 0.025f + ((gang.Wealth / 100) * .000075f);
                if (InterestRate > 0.1) InterestRate = 0.1f;
                gang.Wealth *= 1 + InterestRate;
            }
            using (var db = new DbContext())
            {
                await db.SaveChangesAsync();
            }
        }

        private void AutoUnmute()
        {
            Timer t = new Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);
            t.AutoReset = true;
            t.Elapsed += new ElapsedEventHandler(OnTimedAutoUnmute);
            t.Start();
        }

        private async void OnTimedAutoUnmute(object source, ElapsedEventArgs e)
        {
            foreach (Mute mute in BaseRepository<Mute>.GetAll())
            {
                if (DateTimeOffset.Now.Subtract(mute.MutedAt).TotalMilliseconds > mute.MuteLength.TotalMilliseconds)
                {
                    var guild = _client.GetGuild(mute.GuildId);
                    if (guild != null && guild.GetUser(mute.userid) != null)
                    {
                        var guildData = await GuildRepository.FetchGuildAsync(guild.Id);
                        var mutedRole = guild.GetRole(guildData.MutedRoleId);
                        if (mutedRole != null && guild.GetUser(mute.userid).Roles.Any(x => x.Id == mutedRole.Id))
                        {
                            var channel = guild.GetTextChannel(guildData.ModLogId);
                            if (channel != null && guild.CurrentUser.GuildPermissions.EmbedLinks &&
                                (guild.CurrentUser as IGuildUser).GetPermissions(channel as SocketTextChannel).SendMessages
                                && (guild.CurrentUser as IGuildUser).GetPermissions(channel as SocketTextChannel).EmbedLinks)
                            {
                                await guild.GetUser(mute.userid).RemoveRoleAsync(mutedRole);
                                var footer = new EmbedFooterBuilder()
                                {
                                    IconUrl = "http://i.imgur.com/BQZJAqT.png",
                                    Text = $"Case #{guildData.CaseNumber}"
                                };
                                var builder = new EmbedBuilder()
                                {
                                    Color = new Color(12, 255, 129),
                                    Description = $"**Action:** Automatic Unmute\n**User:** {guild.GetUser(mute.userid)} ({guild.GetUser(mute.userid).Id})",
                                    Footer = footer
                                }.WithCurrentTimestamp();
                                await GuildRepository.ModifyAsync(x => { x.CaseNumber++; return Task.CompletedTask; }, guild.Id);
                                await channel.SendMessageAsync("", embed: builder);
                            }
                        }
                    }
                    await MuteRepository.RemoveMuteAsync(mute.userid, mute.GuildId);
                }
            }
        }

    }
}
