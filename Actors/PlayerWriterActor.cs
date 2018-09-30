using System;
using agileBall_proc.Entities;
using agileBall_proc.Messages;
using Akka;
using Akka.Actor;
using MongoDB.Bson;
using MongoDB.Driver;

namespace agileBall_proc.Actors
{

    public class PlayerWriterActor : ReceiveActor
    {
        private IMongoCollection<Player> _coll;
        public PlayerWriterActor(IMongoCollection<Player> coll) {
            _coll = coll;

            Receive<Player>(p => {
                _coll.ReplaceOne(item => item.Id == p.Id, p, new UpdateOptions {IsUpsert= true});
            });
        }
        public static Props Props(IMongoCollection<Player> coll)
        {
            return Akka.Actor.Props.Create(() => new PlayerWriterActor(coll));
        }

    }
}