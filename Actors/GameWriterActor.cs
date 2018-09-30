using System;
using agileBall_proc.Entities;
using agileBall_proc.Messages;
using Akka;
using Akka.Actor;
using MongoDB.Bson;
using MongoDB.Driver;

namespace agileBall_proc.Actors
{

    public class GameWriterActor : ReceiveActor
    {
        private IMongoCollection<Game> _coll;
        public GameWriterActor(IMongoCollection<Game> coll)
        {
            _coll = coll;

            Receive<Game>(p =>
            {
                _coll.ReplaceOne(item => item.Id == p.Id, p, new UpdateOptions { IsUpsert = true });
            });
        }
        public static Props Props(IMongoCollection<Game> coll)
        {
            return Akka.Actor.Props.Create(() => new GameWriterActor(coll));
        }

    }
}