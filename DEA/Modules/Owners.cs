using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using DEA.SQLite.Repository;

namespace DEA.Modules
{
    public class Owners : ModuleBase<SocketCommandContext>
    {
        [Command("Reset")]
        [Require(Attributes.ServerOwner)]
        [Alias("Reset")]
        [Summary("Resets all cooldowns for a specific user.")]
        [Remarks("Reset [@User]")]
        public async Task ResetCooldowns(IGuildUser user = null)
        {
            var time = DateTimeOffset.Now.AddYears(-1);
            await UserRepository.ModifyAsync(x => {
                x.Whore = time;
                x.Jump = time;
                x.Steal = time;
                x.Rob = time;
                x.Message = time;
                x.Withdraw = time;
                return Task.CompletedTask;
            }, Context);
            await ReplyAsync($"Successfully reset all of {user.Mention} cooldowns.");
        }

        [Command("Give")]
        [Require(Attributes.ServerOwner)]
        [Summary("Inject cash into a user's balance.")]
        [Remarks("Give <@User> <Amount of cash>")]
        public async Task Give(IGuildUser userMentioned, double money) {
            await UserRepository.EditCashAsync(Context, userMentioned.Id, +money);
            await ReplyAsync($"Successfully given {money.ToString("C", Config.CI)} to {userMentioned.Mention}.");
        }

        [Command("Setrate")]
        [Require(Attributes.ServerOwner)]
        [Remarks("$Setrate <@User> <amount>")]
        [Summary("Sets the rate of any user.")]
        public async Task SetRate(IGuildUser user, double rate)
        {
            await UserRepository.ModifyAsync(x => { x.TemporaryMultiplier = rate; return Task.CompletedTask; }, Context);
            await ReplyAsync($"Successfully set {user}'s rate to {rate.ToString("C", Config.CI)}");
        }
    }
}
