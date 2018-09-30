using System;
using Akka;
using Akka.Actor;
using agileBall_proc.Messages;
using agileBall_proc.Entities;
using System.Globalization;

namespace agileBall_proc.Actors
{
    public class TeamActor : ReceiveActor
    {
        private Team t;
        private IActorRef _writer;

        public readonly string Id;

        public TeamActor(string id, IActorRef writer)
        {
            _writer = writer;
            Id = id;
            t = new Team
            {
                Id = id
            };

            Receive<GameResult>(msg =>
            {
                t.Games += 1;
                if (msg.IsAWin)
                {
                    t.Wins += 1;
                }
                else
                {
                    t.Losses += 1;
                }
                writer.Tell(t);
            });
        }

        public static Props Props(string id, IActorRef writer)
        {
            return Akka.Actor.Props.Create(() => new TeamActor(id, writer));
        }
    }
}
