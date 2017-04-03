using Model = Discord.API.Gateway.Reaction;

namespace Discord.WebSocket
{
    public class SocketReaction : IReaction
    {
        public ulong userid { get; }
        public Optional<IUser> User { get; }
        public ulong MessageId { get; }
        public Optional<SocketUserMessage> Message { get; }
        public ISocketMessageChannel Channel { get; }
        public Emoji Emoji { get; }

        internal SocketReaction(ISocketMessageChannel channel, ulong messageId, Optional<SocketUserMessage> message, ulong userid, Optional<IUser> user, Emoji emoji)
        {
            Channel = channel;
            MessageId = messageId;
            Message = message;
            userid = userid;
            User = user;
            Emoji = emoji;
        }
        internal static SocketReaction Create(Model model, ISocketMessageChannel channel, Optional<SocketUserMessage> message, Optional<IUser> user)
        {
            return new SocketReaction(channel, model.MessageId, message, model.userid, user, new Emoji(model.Emoji.Id, model.Emoji.Name));
        }
    }
}
