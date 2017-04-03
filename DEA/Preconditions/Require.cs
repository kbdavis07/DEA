
using System;
using System.Linq;
using System.Threading.Tasks;
using DEA.DAL.Repository;

namespace Discord.Commands
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequireAttribute : PreconditionAttribute
    {
        private Attributes attribute;

        public RequireAttribute(Attributes attribute)
        {
            this.attribute = attribute;
        }
        
        public override async Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IDependencyMap map)
        {
            var user = context.User as IGuildUser;
            var dbUser = await UserRepository.FetchUserAsync(context as SocketCommandContext);
            var guild = await GuildRepository.FetchGuildAsync(context.Guild.Id);
            switch (attribute) {
                case Attributes.BotOwner:
                    if (!Config.CREDENTIALS.OwnerIds.Any(x => x == context.User.Id))
                        return PreconditionResult.FromError("Only an owner of this bot may use this command.");
                    break;
                case Attributes.ServerOwner:
                    if (user.Guild.OwnerId != user.Id)
                        return PreconditionResult.FromError("Only the owner of this server may use this command.");
                    break;
                case Attributes.Admin:
                    if (!(context.User as IGuildUser).GuildPermissions.Administrator)
                        return PreconditionResult.FromError("The administrator permission is required to use this command.");
                    break;
                case Attributes.Moderator:
                    if (!user.RoleIds.Any(x => guild.ModRoles.Any(y => y == x)))
                        return PreconditionResult.FromError("Only a moderator may use this command.");
                    break;
                case Attributes.Nsfw:
                    if (!guild.Nsfw)
                        return PreconditionResult.FromError($"This command may not be used while NSFW is disabled. An administrator may enable with the " +
                                                            $"`{guild.Prefix}ChangeNSFWSettings` command.");
                    var nsfwChannel = await context.Guild.GetChannelAsync(guild.NsfwId);
                    if (nsfwChannel != null && context.Channel.Id != guild.NsfwId)
                        return PreconditionResult.FromError($"You may only use this command in {(nsfwChannel as ITextChannel).Mention}.");
                    var nsfwRole = context.Guild.GetRole(guild.NsfwRoleId);
                    if (nsfwRole != null && (context.User as IGuildUser).RoleIds.All(x => x != guild.NsfwRoleId))
                        return PreconditionResult.FromError($"You do not have permission to use this command.\nRequired role: {nsfwRole.Mention}");
                    break;
                case Attributes.InGang:
                    if (!(await GangRepository.InGangAsync(context.User.Id, context.Guild.Id)))
                        return PreconditionResult.FromError("You must be in a gang to use this command.");
                    break;
                case Attributes.NoGang:
                    if (await GangRepository.InGangAsync(context.User.Id, context.Guild.Id))
                        return PreconditionResult.FromError("You may not use this command while in a gang.");
                    break;
                case Attributes.GangLeader:
                    if (!(await GangRepository.InGangAsync(context.User.Id, context.Guild.Id)))
                        return PreconditionResult.FromError("You must be in a gang to use this command.");
                    break;
                case Attributes.Jump:
                    if (dbUser.Cash < guild.JumpRequirement)
                        return PreconditionResult.FromError($"You do not have the permission to use this command.\nRequired cash: {guild.JumpRequirement.ToString("C", Config.CI)}.");
                    break;
                case Attributes.Steal:
                    if (dbUser.Cash < guild.StealRequirement)
                        return PreconditionResult.FromError($"You do not have the permission to use this command.\nRequired cash: {guild.StealRequirement.ToString("C", Config.CI)}.");
                    break;
                case Attributes.Bully:
                    if (dbUser.Cash < guild.BullyRequirement)
                        return PreconditionResult.FromError($"You do not have the permission to use this command.\nRequired cash: {guild.BullyRequirement.ToString("C", Config.CI)}.");
                    break;
                case Attributes.Rob:
                    if (dbUser.Cash < guild.RobRequirement)
                        return PreconditionResult.FromError($"You do not have the permission to use this command.\nRequired cash: {guild.RobRequirement.ToString("C", Config.CI)}.");
                    break;
                case Attributes.FiftyX2:
                    if (dbUser.Cash < guild.FiftyX2Requirement)
                        return PreconditionResult.FromError($"You do not have the permission to use this command.\nRequired cash: {guild.FiftyX2Requirement.ToString("C", Config.CI)}.");
                    break;
                default:
                    throw new Exception($"ERROR: The {attribute} attribute does not exist!");
            }
            return PreconditionResult.FromSuccess();
        }
    }
}