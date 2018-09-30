using System;

namespace agileBall_proc.Messages
{
    public class GameEnded
    {
        public string Winner { get; set; }
        public string Loser { get; set; }
    }
}