using System;
using Akka;
using Akka.Actor;
using agileBall_proc.Messages;
using agileBall_proc.Entities;
using System.Globalization;

namespace agileBall_proc.Actors
{
    public class GameActor : ReceiveActor
    {
        private Game g;
        private IActorRef _writer;

        public readonly string GameId;
        // public string HomeTeam { get; private set; }
        // public string VisitingTeam { get; private set; }
        // public int HomeScore { get; private set; }
        // public int VisitorScore { get; private set; }
        // public int HomeHits { get; private set; }
        // public int VisitorHits { get; private set; }
        // public string Winner { get; private set; }
        public GameActor(string id, string home, string visitor, IActorRef gameWriter)
        {
            _writer = gameWriter;
            GameId = id;
            g = new Game
            {
                Id = id,
                HomeTeam = home,
                VisitingTeam = visitor,

            };
            // HomeTeam = home;
            // VisitingTeam = visitor;
            // HomeScore = 0; HomeHits = 0; VisitorScore = 0; VisitorHits = 0;
            // Winner = String.Empty;

            Receive<GameEvent>(msg =>
            {
                if (msg.IsHomeAtBat.Equals("1"))
                {
                    //home is at bat
                    g.HomeScore = Convert.ToInt32(msg.HomeScore);
                    g.HomeHits += Convert.ToInt32(msg.HitValue) > 0 ? 1 : 0;
                }
                else
                {
                    //visitor is at bat
                    g.VisitorScore = Convert.ToInt32(msg.VisitorScore);
                    g.VisitorHits += Convert.ToInt32(msg.HitValue) > 0 ? 1 : 0;
                }

                // Console.WriteLine($"{GameId} It's {g.HomeTeam} with {g.HomeScore} vs. {g.VisitingTeam} with {g.VisitorScore}");
                if (msg.IsEndOfGame.Equals("T"))
                {
                    g.IsFinal = true;
                    g.Winner = g.HomeScore > g.VisitorScore ? g.HomeTeam : g.VisitingTeam;

                    Sender.Tell(new GameEnded
                    {
                        Winner = g.Winner,
                        Loser = g.HomeScore > g.VisitorScore ? g.VisitingTeam : g.HomeTeam
                    });
                    // Console.WriteLine($"{g.Winner} WINS by the score of {g.HomeScore} to {g.VisitorScore}");
                }

                _writer.Tell(g);
            });

        }
        public static Props Props(string id, string home, string visitor, IActorRef gameWriter)
        {
            return Akka.Actor.Props.Create(() => new GameActor(id, home, visitor, gameWriter));
        }
    }
}