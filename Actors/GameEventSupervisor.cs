using System;
using agileBall_proc.Messages;
using Akka;
using Akka.Actor;

namespace agileBall_proc.Actors
{
    public class GameEventSupervisor : ReceiveActor
    {
        public GameEventSupervisor()
        {
            Receive<GameEvent>(msg =>
            {
                var playerActor = Context.Child(msg.BatterId);
                if (playerActor.IsNobody())
                {
                    playerActor = Context.ActorOf(PlayerActor.Props(msg.BatterId), msg.BatterId);
                }
                playerActor.Tell(msg);

                var gameActor = Context.Child(msg.GameId);
                if (gameActor.IsNobody())
                {
                    gameActor = Context.ActorOf(GameActor.Props(msg.GameId, msg.Home, msg.Visitor), msg.GameId);
                }
                gameActor.Tell(msg);
            });
        }
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new GameEventSupervisor());
        }

    }
}