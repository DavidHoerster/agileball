using System;
using agileBall_proc.Entities;
using agileBall_proc.Messages;
using Akka;
using Akka.Actor;
using MongoDB.Bson;
using MongoDB.Driver;

namespace agileBall_proc.Actors
{

    public class PlayerActor : ReceiveActor
    {
        private IActorRef _writer;
        public readonly string PlayerId;
        // public int AtBats { get; private set; }
        // public int Hits { get; private set; }
        // public int HomeRuns { get; private set; }
        // public int Rbi { get; private set; }

        public PlayerActor(string id, IActorRef writer)
        {
            PlayerId = id;
            // Hits = 0;
            // AtBats = 0;
            // HomeRuns = 0;
            // Rbi = 0;
            _writer = writer;
            Player p = new Player
            {
                Id = id
            };

            Receive<GameEvent>(msg =>
            {

                p.AtBats += msg.IsAtBat.Equals("T") ? 1 : 0;
                p.Hits += (Convert.ToInt32(msg.HitValue) > 0) ? 1 : 0;
                p.HomeRuns += (Convert.ToInt32(msg.HitValue) == 4) ? 1 : 0;
                p.Rbis += Convert.ToInt32(msg.RbiOnPlay);

                _writer.Tell(p);

                // Console.WriteLine($"Player {p.Id} has {p.Hits} hits in {p.AtBats} at bats with {p.HomeRuns} homers and {p.Rbis} RBI's");
            });
        }
        public static Props Props(string id, IActorRef writer)
        {
            return Akka.Actor.Props.Create(() => new PlayerActor(id, writer));
        }
    }
}