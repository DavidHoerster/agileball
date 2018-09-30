using System;
using agileBall_proc.Messages;
using agileBall_proc.Entities;
using Akka;
using Akka.Actor;

using MongoDB;
using MongoDB.Driver;

namespace agileBall_proc.Actors
{
    public class GameEventSupervisor : ReceiveActor
    {
        private IMongoClient client;
        public GameEventSupervisor()
        {
            client = new MongoClient("mongodb://localhost:C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==@localhost:10255/admin?ssl=true");
            var db = client.GetDatabase("baseball");
            IMongoCollection<Player> playerColl = db.GetCollection<Player>("player");
            IMongoCollection<Game> gameColl = db.GetCollection<Game>("game");
            IMongoCollection<Team> teamColl = db.GetCollection<Team>("team");

            IActorRef playerWriter = Context.ActorOf(PlayerWriterActor.Props(playerColl), "player-writer");
            IActorRef gameWriter = Context.ActorOf(GameWriterActor.Props(gameColl), "game-writer");
            IActorRef teamWriter = Context.ActorOf(TeamWriterActor.Props(teamColl), "team-writer");
            Receive<GameEvent>(msg =>
            {
                var playerActor = Context.Child(msg.BatterId);
                if (playerActor.IsNobody())
                {
                    playerActor = Context.ActorOf(PlayerActor.Props(msg.BatterId, playerWriter), msg.BatterId);
                }
                playerActor.Tell(msg);

                var gameActor = Context.Child(msg.GameId);
                if (gameActor.IsNobody())
                {
                    gameActor = Context.ActorOf(GameActor.Props(msg.GameId, msg.Home, msg.Visitor, gameWriter), msg.GameId);
                }
                gameActor.Tell(msg);
            });

            Receive<GameEnded>(msg =>
            {
                var winner = Context.Child(msg.Winner);
                var loser = Context.Child(msg.Loser);

                if (winner.IsNobody())
                {
                    winner = Context.ActorOf(TeamActor.Props(msg.Winner, teamWriter), msg.Winner);
                }
                if (loser.IsNobody())
                {
                    loser = Context.ActorOf(TeamActor.Props(msg.Loser, teamWriter), msg.Loser);
                }

                winner.Tell(new GameResult
                {
                    Team = msg.Winner,
                    IsAWin = true
                });
                loser.Tell(new GameResult
                {
                    Team = msg.Loser,
                    IsAWin = false
                });
            });
        }
        public static Props Props()
        {
            return Akka.Actor.Props.Create(() => new GameEventSupervisor());
        }

    }
}