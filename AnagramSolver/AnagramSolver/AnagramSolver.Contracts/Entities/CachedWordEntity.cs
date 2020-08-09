namespace AnagramSolver.Contracts.Entities
{
    public class CachedWordEntity
    {
        public int ID { get; set; }
        public string Phrase { get; set; }
        public string AnagramsIds { get; set; }
    }
}
