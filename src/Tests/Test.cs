using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEA.DAL.EF;
using DEA.DAL.Repository;

namespace DEA.Tests
{
    public static class Test
    {
        public static void Guild()
        {
            var db = new DEAContext();

            var NewGuild = new guild();
            Random randNum = new Random();
            var Num1 = randNum.Next(100000000, 999999999);
            var Num2 = randNum.Next(100000000, 999999999);
            ulong GuildId = (ulong)(Num1 + Num2);
            NewGuild.id = GuildId;
            db.guilds.Add(NewGuild);
            db.SaveChanges();
            Console.WriteLine("Saved New Guild");
            var NewGuildId = NewGuild.id;

            Console.WriteLine(NewGuildId);
            var ResultsGuild = GuildRepository.FetchGuildByIdAsync(999999999123456787);

            Console.WriteLine(ResultsGuild.Result.id);
        }
    }
}
