using Discord.Commands;
using System;
using System.Threading.Tasks;
using DEA.SQLite.Repository;

namespace DEA.Modules
{
    public class Gambling : ModuleBase<SocketCommandContext>
    {

        [Command("40+")]
        [Summary("Roll 40 or higher on a 100 sided die, win 1.5X your bet.")]
        [Remarks("40+ <Bet>")]
        public async Task XHalf(double bet)
        {
            await Gamble(bet, 40, 0.5f);
        }

        [Command("50x2")]
        [Require(Attributes.FiftyX2)]
        [Summary("Roll 50 or higher on a 100 sided die, win 2X your bet.")]
        [Remarks("50x2 <Bet>")]
        public async Task X2BetterOdds(double bet)
        {
            await Gamble(bet, 50, 1);
        }

        [Command("55x2")]
        [Summary("Roll 55 or higher on a 100 sided die, win 2X your bet.")]
        [Remarks("55x2 <Bet>")]
        public async Task X2(double bet)
        {
            await Gamble(bet, 55, 1);
        }

        [Command("75+")]
        [Summary("Roll 75 or higher on a 100 sided die, win 3.6X your bet.")]
        [Remarks("75+ <Bet>")]
        public async Task X3dot6(double bet)
        {
            await Gamble(bet, 75, 3.6f);
        }

        [Command("100x90")]
        [Remarks("100x90 <Bet>")]
        [Summary("Roll 100 on a 100 sided die, win 90X your bet.")]
        public async Task X90(double bet) {
            await Gamble(bet, 100, 90);
        }

        private async Task Gamble(double bet, int odds, double payoutMultiplier)
        {
            var user = await UserRepository.FetchUserAsync(Context);
            var guild = await GuildRepository.FetchGuildAsync(Context.Guild.Id);
            if (Context.Guild.GetTextChannel(guild.GambleId) != null && Context.Channel.Id != guild.GambleId)
                throw new Exception($"You may only gamble in {Context.Guild.GetTextChannel(guild.GambleId).Mention}!");
            if (bet < Config.BET_MIN) throw new Exception($"Lowest bet is {Config.BET_MIN}$.");
            if (bet > user.cash) throw new Exception($"You do not have enough money. Balance: {user.cash.ToString("C", Config.CI)}.");
            int roll = new Random().Next(1, 101);
            if (roll >= odds)
            {
                await UserRepository.EditCashAsync(Context, (bet * payoutMultiplier));
                await ReplyAsync($"{Context.User.Mention}, you rolled: {roll}. Congratulations, you just won {(bet * payoutMultiplier).ToString("C", Config.CI)}! " +
                                 $"Balance: {user.cash.ToString("C", Config.CI)}.");
            }
            else
            {
                await UserRepository.EditCashAsync(Context, -bet);
                await ReplyAsync($"{Context.User.Mention}, you rolled {roll}. Unfortunately, you lost {bet.ToString("C", Config.CI)}. " +
                                 $"Balance: {user.cash.ToString("C", Config.CI)}.");
            }
        }
    }
}
