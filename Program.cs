using System;
using System.Collections.Generic;
using System.IO;
using agileBall_proc.Actors;
using agileBall_proc.Messages;
using Akka;
using Akka.Actor;
using Akka.Configuration;
using LumenWorks.Framework.IO.Csv;

namespace agileBall_proc
{
    class Program
    {
        static void Main(string[] args)
        {
            IActorRef gameSupervisor;

            using (var system = ActorSystem.Create("baseball-actors"))
            using (var csv = new CachedCsvReader(new StreamReader(".\\Files\\2017ReallyReduced.csv"), true))
            {
                gameSupervisor = system.ActorOf<GameEventSupervisor>("game-super");
                while (csv.ReadNextRecord())
                {
                    if (csv["Home"].Equals("PIT") || csv["Visitor"].Equals("PIT"))
                    {
                        var evt = new GameEvent
                        {
                            GameId = csv["GameId"],
                            Home = csv["Home"],
                            Visitor = csv["Visitor"],
                            Inning = csv["Inning"],
                            IsHomeAtBat = csv["IsHomeAtBat"],
                            Outs = csv["Outs"],
                            Balls = csv["Balls"],
                            Strikes = csv["Strikes"],
                            VisitorScore = csv["VisitorScore"],
                            HomeScore = csv["HomeScore"],
                            BatterId = csv["BatterId"],
                            PitcherId = csv["PitcherId"],
                            EventTypeId = csv["EventTypeId"],
                            DoesEventEndAtBat = csv["DoesEventEndAtBat"],
                            IsAtBat = csv["IsAtBat"],
                            HitValue = csv["HitValue"],
                            OutsOnPlay = csv["OutsOnPlay"],
                            RbiOnPlay = csv["RbiOnPlay"],
                            NumberOfErrors = csv["NumberOfErrors"],
                            IsNewGame = csv["IsNewGame"],
                            IsEndOfGame = csv["IsEndOfGame"],
                            EventId = csv["EventId"],
                            Date = csv["Date"],
                            GameForDay = csv["GameForDay"]
                        };

                        gameSupervisor.Tell(evt);
                    }
                }
                Console.Read();
            }
        }
    }
}
