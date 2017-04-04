using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DEA.Database.Repository;
using Discord.Addons.InteractiveCommands;

namespace DEA.Services
{
    public class MessageRecieved
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private IDependencyMap _map;

        public async Task InitializeAsync(DiscordSocketClient c, IDependencyMap map)
        {
            _client = c;
            _service = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Sync
            });

            await _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _map = map;
            _map.Add(new InteractiveService(_client));

            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;

            PrettyConsole.NewLine(msg.Content);

            

            var Context = new SocketCommandContext(_client, msg);

            PrettyConsole.NewLine((await GuildRepository.FetchGuildAsync(Context.Guild.Id)).Prefix);

            if (Context.User.IsBot) return;

            if (!(Context.Channel is SocketTextChannel)) return;

            if (!(Context.Guild.CurrentUser as IGuildUser).GetPermissions(Context.Channel as SocketTextChannel).SendMessages) return;

            int argPos = 0;
            string prefix = (await GuildRepository.FetchGuildAsync(Context.Guild.Id)).Prefix;
            if (msg.HasStringPrefix(prefix, ref argPos) ||
                msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                PrettyConsole.Log(LogSeverity.Debug, $"Guild: {Context.Guild.Name}, User: {Context.User}", msg.Content);
                var result = await _service.ExecuteAsync(Context, argPos, _map);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    var cmd = _service.Search(Context, argPos).Commands.First().Command;
                    if (result.ErrorReason.Length == 0) return;
                    switch (result.Error)
                    {
                        case CommandError.BadArgCount:
                            await msg.Channel.SendMessageAsync($"{Context.User.Mention}, You are incorrectly using this command. Usage: `{prefix}{cmd.Remarks}`");
                            break;
                        case CommandError.ParseFailed:
                            await msg.Channel.SendMessageAsync($"{Context.User.Mention}, Invalid number.");
                            break;
                        default:
                            await msg.Channel.SendMessageAsync($"{Context.User.Mention}, {result.ErrorReason}");
                            break;
                    }
                }
            }
            else if (msg.ToString().Length >= Config.MIN_CHAR_LENGTH && !msg.ToString().StartsWith(":"))
            {
                var user = await UserRepository.FetchUserAsync(Context);
                var rate = Config.TEMP_MULTIPLIER_RATE;
                if (DateTimeOffset.Now.Subtract(user.Message).TotalMilliseconds > user.MessageCooldown.TotalMilliseconds)
                {
                    await UserRepository.ModifyAsync(x => {
                        x.Cash += user.TemporaryMultiplier * user.InvestmentMultiplier;
                        x.TemporaryMultiplier = user.TemporaryMultiplier + rate;
                        x.Message = DateTimeOffset.Now;
                        return Task.CompletedTask;
                    }, Context);
                    await RankHandler.Handle(Context.Guild, Context.User.Id);
                }
            }
        }
    }
}
