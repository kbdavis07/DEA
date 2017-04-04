using System;
using DEA.DAL.EF;
using DEA.DAL.Repository;

namespace DEA
{
    public class Program
    {
        //public static void Main(string[] args) => new DEABot().RunAndBlockAsync(args).GetAwaiter().GetResult();


       public static void Main(string[] args)
        {

            Test();
            
        }

       
         public static void Test()
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




