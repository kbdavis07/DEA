using DEA.Database.Repository;
using DEA.Events;
using DEA.Resources;
using DEA.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DEA
{
    public class DEABot
    {
        public static Credentials Credentials { get; private set; }
        public static CommandService CommandService { get; private set; }
        public static DiscordSocketClient Client { get; private set; }

        static DEABot()
        {
            try
            {
                using (StreamReader file = File.OpenText(@"..\..\Credentials.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    Credentials = (Credentials)serializer.Deserialize(file, typeof(Credentials));
                }
            }
            catch (IOException e)
            {
                PrettyConsole.Log(LogSeverity.Error, "Error while loading up Credentials.json, please fix this issue and restart the bot", e.Message);
            }
        }

        public async Task RunAsync(params string[] args)
        {
            PrettyConsole.NewLine("===   DEA   ===");
            PrettyConsole.NewLine();

            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Error,
                MessageCacheSize = 10,
                TotalShards = Credentials.ShardCount,
                //AlwaysDownloadUsers = true,
            });

            Client.Log += (l)
                => Task.Run(()
                => PrettyConsole.Log(l.Severity, l.Source, l.Exception?.ToString() ?? l.Message));

            CommandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Sync
            });

            var sw = Stopwatch.StartNew();
            //Connection
            await Client.LoginAsync(TokenType.Bot, Credentials.Token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            //await Client.DownloadAllUsersAsync().ConfigureAwait(false);
            sw.Stop();
            PrettyConsole.Log(LogSeverity.Info, "Successfully connected", $"Elapsed time: {sw.Elapsed.TotalSeconds.ToString()} seconds.");

            var Map = new DependencyMap();
            ConfigureServices(Map);
            await new MessageRecieved().InitializeAsync(Client, Map);
            new Ready(Client);
            PrettyConsole.Log(LogSeverity.Info, "Events and mapping successfully initialized", $"Client ready.");

            using (var db = new DEAContext())
            {
                await db.Database.EnsureCreatedAsync();
            }
            PrettyConsole.Log(LogSeverity.Info, "Database creation ensured", $"Ready for use.");
        }

        public async Task RunAndBlockAsync(params string[] args)
        {
            await RunAsync(args).ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }

        private void ConfigureServices(IDependencyMap map)
        {
            map.Add(Client);
        }

    }
}
