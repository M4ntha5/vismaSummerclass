using System;

namespace AnagramSolver.Contracts.Entities
{
    public class UserLogEntity
    {
        public int ID { get; set; }
        public string Ip { get; set; }
        public string Phrase { get; set; }
        public TimeSpan SearchTime { get; set; }
    }
}
