using Discord.Commands;
using System;
using System.Threading.Tasks;
using DEA.Database.Repository;

namespace DEA.Modules
{
    public class Gambling : ModuleBase<SocketCommandContext>
    {

        [Command("21+")]
        [Summary("Roll 20.83 or higher on a 100.00 sided die, win 0.2X your bet.")]
        [Remarks("21+ <Bet>")]
        public async Task XHalf(double bet)
        {
            await Gamble(bet, 20.83, 0.2);
        }

        [Command("50x2")]
        [Require(Attributes.FiftyX2)]
        [Summary("Roll 50.00 or higher on a 100.00 sided die, win your bet.")]
        [Remarks("50x2 <Bet>")]
        public async Task X2BetterOdds(double bet)
        {
            await Gamble(bet, 50.0, 1.0);
        }

        [Command("53x2")]
        [Summary("Roll 52.50 or higher on a 100.00 sided die, win your bet.")]
        [Remarks("53x2 <Bet>")]
        public async Task X2(double bet)
        {
            await Gamble(bet, 52.5, 1.0);
        }

        [Command("75+")]
        [Summary("Roll 75.00 or higher on a 100.00 sided die, win 2.8X your bet.")]
        [Remarks("75+ <Bet>")]
        public async Task X3dot8(double bet)
        {
            await Gamble(bet, 75.0, 2.8);
        }

        [Command("100x9499")]
        [Remarks("100x9499 <Bet>")]
        [Summary("Roll 100.00 on a 100.00 sided die, win 9499X your bet.")]
        public async Task X90(double bet) {
            await Gamble(bet, 100.0, 9499.0);
        }

        private async Task Gamble(double bet, double odds, double payoutMultiplier)
        {
            var user = await UserRepository.FetchUserAsync(Context);
            var guild = await GuildRepository.FetchGuildAsync(Context.Guild.Id);
            if (Context.Guild.GetTextChannel((ulong)guild.GambleId) != null && Context.Channel.Id != guild.GambleId)
                throw new Exception($"You may only gamble in {Context.Guild.GetTextChannel((ulong)guild.GambleId).Mention}!");
            if (bet < Config.BET_MIN) throw new Exception($"Lowest bet is {Config.BET_MIN}$.");
            if (bet > user.Cash) throw new Exception($"You do not have enough money. Balance: {user.Cash.ToString("C", Config.CI)}.");
            double roll = new Random().Next(1, 10001) / 100.0;
            if (roll >= odds * 100)
            {
                await UserRepository.EditCashAsync(Context, (bet * payoutMultiplier));
                await ReplyAsync($"{Context.User.Mention}, you rolled: {roll.ToString("N2")}. Congrats, you won {(bet * payoutMultiplier).ToString("C", Config.CI)}! " +
                                 $"Balance: {user.Cash.ToString("C", Config.CI)}.");
            }
            else
            {
                await UserRepository.EditCashAsync(Context, -bet);
                await ReplyAsync($"{Context.User.Mention}, you rolled: {roll.ToString("N2")}. Unfortunately, you lost {bet.ToString("C", Config.CI)}. " +
                                 $"Balance: {user.Cash.ToString("C", Config.CI)}.");
            }
        }
    }
}
