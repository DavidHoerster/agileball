namespace agileBall_proc.Messages
{
    public class GameEvent
    {
        public string GameId { get; set; }
        public string Home { get; set; }
        public string Visitor { get; set; }
        public string Inning { get; set; }
        public string IsHomeAtBat { get; set; }
        public string Outs { get; set; }
        public string Balls { get; set; }
        public string Strikes { get; set; }
        public string VisitorScore { get; set; }
        public string HomeScore { get; set; }
        public string BatterId { get; set; }
        public string PitcherId { get; set; }
        public string EventTypeId { get; set; }
        public string DoesEventEndAtBat { get; set; }
        public string IsAtBat { get; set; }
        public string HitValue { get; set; }
        public string OutsOnPlay { get; set; }
        public string RbiOnPlay { get; set; }
        public string NumberOfErrors { get; set; }
        public string IsNewGame { get; set; }
        public string IsEndOfGame { get; set; }
        public string EventId { get; set; }
        public string Date { get; set; }
        public string GameForDay { get; set; }
    }
}