using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEA.DAL.EF;

namespace DEA
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new DEAContext())
            {
                var NewGuild = new guild();

                NewGuild.id = 10000000000000000000;
                
                db.guilds.Add(NewGuild);

                var NewGuildId = db.SaveChanges();
                Console.WriteLine("Saved NewRank Role");
                Console.WriteLine(NewGuildId);
            }

        }
    }
}
