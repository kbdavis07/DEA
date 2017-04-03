using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DEA.SQLite.Repository;

namespace System.Modules
{
    public class System : ModuleBase<SocketCommandContext>
    {

        private Process _process;
        private CommandService _service;

        protected override void BeforeExecute()
        {
            _process = Process.GetCurrentProcess();
        }

        public System(CommandService service)
        {
            _service = service;
        }

        [Command("Information")]
        [Alias("info")]
        [Remarks("Information")]
        [Summary("Information about the DEA Cash System.")]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Info(string investString = null)
        {
            var guild = await GuildRepository.FetchGuildAsync(Context.Guild.Id);
            string p = guild.Prefix;

            var builder = new EmbedBuilder()
            {
                Color = new Color(0x00AE86),
                Description = $@"In order to gain money, you must send a message that is at least {Config.MIN_CHAR_LENGTH} characters in length. There is a 30 second cooldown between each message that will give you cash. However, these rates are not fixed. For every message you send, your chatting multiplier(which increases the amount of money you get per message) is increased by {Config.TEMP_MULTIPLIER_RATE}. This rate is reset every hour.

To view your steadily increasing chatting multiplier, you may use the `{p}rate` command, and the `{p}money` command to see your cash grow. This command shows you every single variable taken into consideration for every message you send. If you wish to improve these variables, you may use investments. With the `{p}investments` command, you may pay to have *permanent* changes to your message rates. These will stack with the chatting multiplier."
            };
            var secondBuilder = new EmbedBuilder()
            {
                Color = new Color(0x00AE86),
                Description = $@"Another common way of gaining money is by gambling, there are loads of different gambling commands, which can all be viewed with the `{p}help` command. You might be wondering what is the point of all these commands. This is where ranks come in. The full list of ranks may be viewed with the `{p}rank` command. Depending on how much money you have, you will get a certain rank, and mainly, gain access to more commands. As your cash stack grows, so do the quantity commands you can use:

**{guild.JumpRequirement.ToString("C", Config.CI)}:** `{p}jump`
**{guild.StealRequirement.ToString("C", Config.CI)}:** `{p}steal`
**{guild.RobRequirement.ToString("C", Config.CI)}:** `{p}rob <Resources>`
**{guild.BullyRequirement.ToString("C", Config.CI)}:** `{p}bully`
**{guild.FiftyX2Requirement.ToString("C", Config.CI)}:** `{p}55x2 <Bet>"
            };
            var channel = await Context.User.CreateDMChannelAsync();
            await channel.SendMessageAsync("", embed: builder);
            await channel.SendMessageAsync("", embed: secondBuilder);
            await ReplyAsync($"{Context.User.Mention}, Information about the DEA Cash System has been DMed to you!");
        }

        [Command("Help")]
        [Alias("commands", "cmd", "cmds", "command")]
        [Summary("All command information.")]
        [Remarks("Help [Command or Module]")]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task HelpAsync(string commandOrModule = null)
        {
            string prefix = (await GuildRepository.FetchGuildAsync(Context.Guild.Id)).Prefix;

            if (commandOrModule != null)
            {
                if (commandOrModule.StartsWith(prefix)) commandOrModule = commandOrModule.Remove(0, prefix.Length);
                foreach (var module in _service.Modules)
                {
                    if (module.Name.ToLower() == commandOrModule.ToLower())
                    {
                        var longestInModule = 0;
                        foreach (var cmd in module.Commands)
                            if (cmd.Aliases.First().Length > longestInModule) longestInModule = cmd.Aliases.First().Length;
                        var moduleInfo = $"**{module.Name} Commands **: ```asciidoc\n";
                        foreach (var cmd in module.Commands)
                        {
                            moduleInfo += $"{prefix}{cmd.Aliases.First()}{new string(' ', (longestInModule + 1) - cmd.Aliases.First().Length)} :: {cmd.Summary}\n";
                        }
                        moduleInfo += "```";
                        await ReplyAsync(moduleInfo);
                        return;
                    }
                }

                foreach (var module in _service.Modules)
                {
                    foreach (var cmd in module.Commands)
                    {
                        foreach (var alias in cmd.Aliases)
                            if (alias == commandOrModule.ToLower())
                            {
                                var command = new EmbedBuilder()
                                {
                                    Title = $"{cmd.Name}",
                                    Color = new Color(0x00AE86),
                                    Description = $"**Description:** {cmd.Summary}\n"
                                };
                                if (cmd.Remarks != null) command.Description += $"**Usage:** `{prefix}{cmd.Remarks}`";
                                await ReplyAsync("", embed: command);
                                return;
                            }
                    }
                }
                string modules = null;
                foreach (var module in _service.Modules) modules += $"{module.Name}, ";
                await ReplyAsync($"This command/module does not exist. Current list of modules: {modules.Substring(0, modules.Length - 2)}.");
            }
            else
            {
                var help = new EmbedBuilder()
                {
                    Title = "Welcome to DEA",
                    Color = new Color(0x00AE86),
                    Description = $@"DEA is a multi-purpose Discord Bot mainly known for it's infamous Cash System with multiple subtleties referencing to the show Narcos, which inspired the creation of this masterpiece.

For all information about command usage and setup on your Discord Sever, view the documentation: <https://realblazeit.github.io/DEA/>

This command may be used for view the commands for each of the following modules: System, Administration, Moderation, General, Gambling and Crime. It may also be used the view the usage of a specific command.

In order to **add DEA to your Discord Server**, click the following link: <https://discordapp.com/oauth2/authorize?client_id={Context.Client.CurrentUser.Id}&scope=bot&permissions=477195286> 

If you have any other questions, you may join the **Official DEA Discord Server:** <https://discord.me/Rush>, a server home to infamous meme events such as a raids and insanity. Join for the dankest community a man could desire."
                };

                var channel = await Context.User.CreateDMChannelAsync();
                await channel.SendMessageAsync("", embed: help);
                await ReplyAsync($"{Context.User.Mention}, you have been DMed with all the command information!");
            }        
        }

        [Command("Stats")]
        [Alias("statistics")]
        [Remarks("Stats")]
        [Summary("All the statistics about DEA.")]
        public async Task Info()
        {
            var uptime = (DateTimeOffset.Now - _process.StartTime);
            var application = await Context.Client.GetApplicationInfoAsync();
            var message =
                $"```asciidoc\n= STATISTICS =\n" +
                $"• Memory   :: {(_process.PrivateMemorySize64 / 1000000f).ToString("0.##")} MB\n" +
                $"• Uptime   :: Days: {uptime.Days}, Hours: {uptime.Hours}, Minutes: {uptime.Minutes}, Seconds: {uptime.Seconds}\n" +
                $"• Users    :: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}\n" +
                $"• Servers  :: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"• Channels :: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}\n" +
                $"• Library  :: Discord.Net {DiscordConfig.Version}\n" +
                $"• Runtime  :: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}```";
            var channel = await Context.User.CreateDMChannelAsync();
            await channel.SendMessageAsync(message);
            await ReplyAsync($"{Context.User.Mention}, you have been DMed with all the statistics!");
        }
    }
}