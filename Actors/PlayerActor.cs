using System;
using agileBall_proc.Messages;
using Akka;
using Akka.Actor;


namespace agileBall_proc.Actors
{

    public class PlayerActor : ReceiveActor
    {
        public readonly string PlayerId;
        public int AtBats { get; private set; }
        public int Hits { get; private set; }
        public int HomeRuns { get; private set; }
        public int Rbi { get; private set; }

        public PlayerActor(string id)
        {
            PlayerId = id;
            Hits = 0;
            AtBats = 0;
            HomeRuns = 0;
            Rbi = 0;

            Receive<GameEvent>(msg =>
            {
                AtBats += msg.IsAtBat.Equals("TRUE") ? 1 : 0;
                Hits += (Convert.ToInt32(msg.HitValue) > 0) ? 1 : 0;
                HomeRuns += (Convert.ToInt32(msg.HitValue) == 4) ? 1 : 0;
                Rbi += Convert.ToInt32(msg.RbiOnPlay);

                Console.WriteLine($"Player {PlayerId} has {Hits} hits in {AtBats} at bats with {HomeRuns} homers and {Rbi} RBI's");
            });
        }
        public static Props Props(string id)
        {
            return Akka.Actor.Props.Create(() => new PlayerActor(id));
        }
    }
}