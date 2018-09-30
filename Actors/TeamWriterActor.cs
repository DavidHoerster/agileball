using System;
using agileBall_proc.Entities;
using agileBall_proc.Messages;
using Akka;
using Akka.Actor;
using MongoDB.Bson;
using MongoDB.Driver;

namespace agileBall_proc.Actors
{

    public class TeamWriterActor : ReceiveActor
    {
        private IMongoCollection<Team> _coll;
        public TeamWriterActor(IMongoCollection<Team> coll)
        {
            _coll = coll;

            Receive<Team>(p =>
            {
                _coll.ReplaceOne(item => item.Id == p.Id, p, new UpdateOptions { IsUpsert = true });
            });
        }
        public static Props Props(IMongoCollection<Team> coll)
        {
            return Akka.Actor.Props.Create(() => new TeamWriterActor(coll));
        }

    }
}