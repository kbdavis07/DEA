using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DEA.DAL.Repository;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DEA.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("Investments")]
        [Alias("Investements", "Investement", "Investment")]
        [Summary("Increase your money per message")]
        [Remarks("Investments [investment]")]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Invest(string investString = null)
        {
            var guild = await GuildRepository.FetchGuildAsync(Context.Guild.Id);
            var user = await UserRepository.FetchUserAsync(Context);
            double cash = user.cash;
            switch (investString)
            {
                case "line":
                    if (Config.LINE_COST > cash)
                    {
                        await ReplyAsync($"{Context.User.Mention}, you do not have enough money. Balance: {cash.ToString("C", Config.CI)}");
                        break;
                    }
                    if (user.messagecooldown == Config.LINE_COOLDOWN)
                    {
                        await ReplyAsync($"{Context.User.Mention}, you have already purchased this investment.");
                        break;
                    }
                    await UserRepository.EditCashAsync(Context, -Config.LINE_COST);
                    await UserRepository.ModifyAsync(x => { x.MessageCooldown = Config.LINE_COOLDOWN; return Task.CompletedTask; }, Context);
                    await ReplyAsync($"{Context.User.Mention}, don't forget to wipe your nose when you are done with that line.");
                    break;
                case "pound":
                case "lb":
                    if (Config.POUND_COST > cash)
                    {
                        await ReplyAsync($"{Context.User.Mention}, you do not have enough money. Balance: {cash.ToString("C", Config.CI)}");
                        break;
                    }
                    if (user.InvestmentMultiplier >= Config.POUND_MULTIPLIER)
                    {
                        await ReplyAsync($"{Context.User.Mention}, you already purchased this investment.");
                        break;
                    }
                    await UserRepository.EditCashAsync(Context, -Config.POUND_COST);
                    await UserRepository.ModifyAsync(x => { x.InvestmentMultiplier = Config.POUND_MULTIPLIER; return Task.CompletedTask; }, Context);
                    await ReplyAsync($"{Context.User.Mention}, ***DOUBLE CASH SMACK DAB CENTER NIGGA!***");
                    break;
                case "kg":
                case "kilo":
                case "kilogram":
                    if (Config.KILO_COST > cash)
                    {
                        await ReplyAsync($"{Context.User.Mention}, you do not have enough money. Balance: {cash.ToString("C", Config.CI)}");
                        break;
                    }
                    if (user.InvestmentMultiplier != Config.POUND_MULTIPLIER)
                    {
                        await ReplyAsync($"{Context.User.Mention}, you must purchase the pound of cocaine investment before buying this one.");
                        break;
                    }
                    if (user.InvestmentMultiplier >= Config.KILO_MULTIPLIER)
                    {
                        await ReplyAsync($"{Context.User.Mention}, you already purchased this investment.");
                        break;
                    }
                    await UserRepository.EditCashAsync(Context, -Config.KILO_COST);
                    await UserRepository.ModifyAsync(x => { x.InvestmentMultiplier = Config.KILO_MULTIPLIER; return Task.CompletedTask; }, Context);
                    await ReplyAsync($"{Context.User.Mention}, only the black jews would actually enjoy 4$/msg.");
                    break;
                default:
                    var builder = new EmbedBuilder()
                    {
                        Title = "Current Available Investments:",
                        Color = new Color(0x0000FF),
                        Description = ($"\n**Cost: {Config.LINE_COST}$** | Command: `{guild.Prefix}investments line` | Description: " +
                        $"One line of blow. Seems like nothing, yet it's enough to lower the message cooldown from 30 to 25 seconds." +
                        $"\n**Cost: {Config.POUND_COST}$** | Command: `{guild.Prefix}investments pound` | Description: " +
                        $"This one pound of coke will double the amount of cash you get per message\n**Cost: {Config.KILO_COST}$** | Command: " +
                        $"`{guild.Prefix}investments kilo` | Description: A kilo of cocaine is more than enough to " +
                        $"quadruple your cash/message.\n These investments stack with the chatting multiplier. However, they do not stack with themselves."),
                    };
                    await ReplyAsync("", embed: builder);
                    break;
            }
        }

        [Command("Leaderboards")]
        [Alias("lb", "rankings", "highscores", "leaderboard", "highscore")]
        [Summary("View the richest Drug Traffickers.")]
        [Remarks("Leaderboards")]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Leaderboards()
        {
            var users = (await UserRepository.AllAsync(Context.Guild.Id)).OrderByDescending(x => x.Cash);
            string message = "```asciidoc\n= The Richest Traffickers =\n";
            int position = 1;
            int longest = 0;

            foreach (User user in users)
            {
                if (Context.Guild.GetUser(user.UserId) == null) continue;
                if ($"{Context.Guild.GetUser(user.UserId)}".Length > longest) longest = $"{position}. {Context.Guild.GetUser(user.UserId)}".Length;
                if (position >= Config.LEADERBOARD_CAP || users.Last().Id == user.Id)
                {
                    position = 1;
                    break;
                }
                position++;
            }

            foreach (User user in users)
            {
                if (Context.Guild.GetUser(user.UserId) == null) continue;
                message += $"{position}. {Context.Guild.GetUser(user.UserId)}".PadRight(longest + 2) +
                           $" :: {user.Cash.ToString("C", Config.CI)}\n";
                if (position >= Config.LEADERBOARD_CAP) break;
                position++;
            }

            await ReplyAsync($"{message}```");
        }

        [Command("Rates")]
        [Alias("highestrate", "ratehighscore", "bestrate", "highestrates", "ratelb", "rateleaderboards")]
        [Summary("View the richest Drug Traffickers.")]
        [Remarks("Rates")]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Chatters()
        {
            var users = (await UserRepository.AllAsync(Context.Guild.Id)).OrderByDescending(x => x.TemporaryMultiplier);
            string message = "```asciidoc\n= The Best Chatters =\n";
            int position = 1;
            int longest = 0;

            SocketGuildUser user;
            foreach (User dbUser in users)
            {
                user = Context.Guild.GetUser(dbUser.UserId);
                if (user == null) continue;
                if ($"{user}".Length > longest) longest = $"{position}. {user}".Length;
                if (position >= Config.RATELB_CAP || users.Last().Id == dbUser.Id)
                {
                    position = 1;
                    break;
                }
                position++;
            }

            foreach (User dbUser in users)
            {
                user = Context.Guild.GetUser(dbUser.UserId);
                if (user == null) continue;
                message += $"{position}. {Context.Guild.GetUser(user.Id)}".PadRight(longest + 2) +
                           $" :: {dbUser.TemporaryMultiplier.ToString("N2")}\n";
                if (position >= Config.RATELB_CAP) break;
                position++;
            }

            await ReplyAsync($"{message}```");
        }

        [Command("Donate")]
        [Alias("Sauce")]
        [Summary("Sauce some cash to one of your mates.")]
        [Remarks("Donate <@User> <Amount of cash>")]
        public async Task Donate(IGuildUser userMentioned, double money)
        {
            var user = await UserRepository.FetchUserAsync(Context);
            if (userMentioned.Id == Context.User.Id) throw new Exception("Hey kids! Look at that retard, he is trying to give money to himself!");
            if (money < Config.DONATE_MIN) throw new Exception($"Lowest donation is {Config.DONATE_MIN}$.");
            if (user.Cash < money) throw new Exception($"You do not have enough money. Balance: {user.Cash.ToString("C", Config.CI)}.");
            await UserRepository.EditCashAsync(Context, -money);
            double deaMoney = money * Config.DEA_CUT / 100;
            await UserRepository.EditCashAsync(Context, userMentioned.Id, money - deaMoney);
            await UserRepository.EditCashAsync(Context, Context.Guild.CurrentUser.Id, deaMoney);
            await ReplyAsync($"Successfully donated {money.ToString("C", Config.CI)} to {userMentioned.Mention}. DEA has taken a {deaMoney.ToString("C", Config.CI)} cut out of this donation.");
        }

        [Command("Money")]
        [Alias("rank", "cash", "ranking", "balance")]
        [Summary("View the wealth of anyone.")]
        [Remarks("Money [@User]")]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Money(IGuildUser userToView = null)
        {
            userToView = userToView ?? Context.User as IGuildUser;
            List<User> users = (await UserRepository.AllAsync(Context.Guild.Id)).OrderByDescending(x => x.Cash).ToList();
            IRole rank = null;
            //TODO: rank = await RankHandler.GetRank(Context.Guild, userToView.Id, Context.Guild.Id);
            var builder = new EmbedBuilder()
            {
                Title = $"Ranking of {userToView}",
                Color = new Color(0x00AE86),
                Description = $"Balance: {(await UserRepository.FetchUserAsync(Context)).Cash.ToString("C", Config.CI)}\n" +
                              $"Position: #{users.FindIndex(x => x.UserId == userToView.Id) + 1}\n"
            };
            if (rank != null)
                builder.Description += $"Rank: {rank.Mention}";
            await ReplyAsync("", embed: builder);
        }

        [Command("Rate")]
        [Summary("View the money/message rate of anyone.")]
        [Remarks("Rate [@User]")]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        public async Task Rate(IGuildUser userToView = null)
        {
            userToView = userToView ?? Context.User as IGuildUser;
            var user = await UserRepository.FetchUserAsync(Context);
            var builder = new EmbedBuilder()
            {
                Title = $"Rate of { userToView }",
                Color = new Color(0x00AE86),
                Description = $"Currently receiving " +
                $"{(user.InvestmentMultiplier * user.TemporaryMultiplier).ToString("C", Config.CI)} " +
                $"per message sent every {user.MessageCooldown.TotalSeconds} seconds that is at least 7 characters long.\n" +
                $"Chatting multiplier: {user.TemporaryMultiplier.ToString("N2")}\nInvestment multiplier: " +
                $"{user.InvestmentMultiplier.ToString("N2")}\nMessage cooldown: " +
                $"{user.MessageCooldown.TotalSeconds} seconds"
            };
            await ReplyAsync("", embed: builder);
        }
    }
}
