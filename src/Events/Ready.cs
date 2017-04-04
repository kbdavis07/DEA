using DEA.Services;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DEA.Events
{
    class Ready
    {
        private DiscordSocketClient _client;

        public Ready(DiscordSocketClient client)
        {
            _client = client;

            _client.Ready += HandleReady;

            new UserEvents(_client);
            new RoleEvents(_client);
            new ChannelEvents(_client);
            new RecurringFunctions(_client);
        }

        private async Task HandleReady()
        {
            await _client.SetGameAsync("USE $help");
        }

    }
}
