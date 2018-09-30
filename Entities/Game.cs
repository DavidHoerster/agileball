using System;

namespace agileBall_proc.Entities
{
    public class Game
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeam { get; set; }
        public string VisitingTeam { get; set; }
        public int HomeScore { get; set; }
        public int VisitorScore { get; set; }
        public int HomeHits { get; set; }
        public int VisitorHits { get; set; }
        public bool IsFinal { get; set; }
        public string Winner { get; set; }
    }
}