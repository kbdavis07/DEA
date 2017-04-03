namespace DEA.Resources
{
    public class Credentials
    {
        public string Token { get; set; }

        public ulong[] OwnerIds { get; set; }

        public int ShardCount { get; set; } = 1;
    }
}