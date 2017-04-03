using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using DEA.DAL.EF;
using DEA.DAL.Repository;
using Discord;
using Discord.WebSocket;

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
            foreach (user user in BaseRepository<user>.GetAll())
                if (user.temporarymultiplier != 1) user.temporarymultiplier = 1;

            using (var db = new DEAContext())
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
            foreach (var gang in BaseRepository<gang>.GetAll())
            {
                var InterestRate = 0.025f + ((gang.wealth / 100) * .000075f);
                if (InterestRate > 0.1) InterestRate = 0.1f;
                gang.wealth *= 1 + InterestRate;
            }
            using (var db = new DEAContext())
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
            foreach (mute mute in BaseRepository<mute>.GetAll())
            {
                if (DateTimeOffset.Now.Subtract(mute.mutedat).TotalMilliseconds > mute.mutelength.TotalMilliseconds)
                {
                    var guild = _client.GetGuild((ulong)mute.guildid);
                    if (guild != null && guild.GetUser((ulong)mute.userid) != null)
                    {
                        var guildData = await GuildRepository.FetchGuildAsync(guild.Id);
                        var mutedRole = guild.GetRole((ulong)guildData.id);
                        if (mutedRole != null && guild.GetUser((ulong)mute.userid).Roles.Any(x => x.Id == mutedRole.Id))
                        {
                            var channel = guild.GetTextChannel((ulong)guildData.modlogid);
                            if (channel != null && guild.CurrentUser.GuildPermissions.EmbedLinks &&
                                (guild.CurrentUser as IGuildUser).GetPermissions(channel as SocketTextChannel).SendMessages
                                && (guild.CurrentUser as IGuildUser).GetPermissions(channel as SocketTextChannel).EmbedLinks)
                            {
                                await guild.GetUser((ulong)mute.userid).RemoveRoleAsync(mutedRole);
                                var footer = new EmbedFooterBuilder()
                                {
                                    IconUrl = "http://i.imgur.com/BQZJAqT.png",
                                    Text = $"Case #{guildData.casenumber}"
                                };
                                var builder = new EmbedBuilder()
                                {
                                    Color = new Color(12, 255, 129),
                                    Description = $"**Action:** Automatic Unmute\n**User:** {guild.GetUser((ulong)mute.userid)} ({guild.GetUser((ulong)mute.userid).Id})",
                                    Footer = footer
                                }.WithCurrentTimestamp();
                                await GuildRepository.ModifyAsync(x => { x.casenumber++; return Task.CompletedTask; }, guild.Id);
                                await channel.SendMessageAsync("", embed: builder);
                            }
                        }
                    }
                    await MuteRepository.RemoveMuteAsync((ulong)mute.userid, (ulong)mute.guildid);
                }
            }
        }

    }
}
