using System;
using Akka;
using Akka.Actor;
using agileBall_proc.Messages;

namespace agileBall_proc.Actors
{
    public class GameActor : ReceiveActor
    {

        public readonly string GameId;
        public string HomeTeam { get; private set; }
        public string VisitingTeam { get; private set; }
        public int HomeScore { get; private set; }
        public int VisitorScore { get; private set; }
        public int HomeHits { get; private set; }
        public int VisitorHits { get; private set; }
        public string Winner { get; private set; }
        public GameActor(string id, string home, string visitor)
        {
            GameId = id;
            HomeTeam = home;
            VisitingTeam = visitor;
            HomeScore = 0; HomeHits = 0; VisitorScore = 0; VisitorHits = 0;
            Winner = String.Empty;

            Receive<GameEvent>(msg =>
            {
                if (msg.IsHomeAtBat.Equals("1"))
                {
                    //home is at bat
                    HomeScore = Convert.ToInt32(msg.HomeScore);
                    HomeHits += Convert.ToInt32(msg.HitValue) > 0 ? 1 : 0;
                }
                else
                {
                    //visitor is at bat
                    VisitorScore = Convert.ToInt32(msg.VisitorScore);
                    VisitorHits += Convert.ToInt32(msg.HitValue) > 0 ? 1 : 0;
                }

                Console.WriteLine($"{GameId} It's {HomeTeam} with {HomeScore} vs. {VisitingTeam} with {VisitorScore}");
                if (msg.IsEndOfGame.Equals("T"))
                {
                    Winner = HomeScore > VisitorScore ? HomeTeam : VisitingTeam;
                    Console.WriteLine($"{Winner} WINS by the score of {HomeScore} to {VisitorScore}");
                }
            });

        }
        public static Props Props(string id, string home, string visitor)
        {
            return Akka.Actor.Props.Create(() => new GameActor(id, home, visitor));
        }
    }
}