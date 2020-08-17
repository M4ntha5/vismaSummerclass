namespace AnagramSolver.Contracts.Utils
{
    public static class Settings
    {
        public static int MinInputLength { get; set; }
        public static int AnagramsToGenerate { get; set; }
        public static string DataFileName { get; set; }
        public static string ConnectionString { get; set; }
        public static string ConnectionStringCodeFirst { get; set; }
        public static string ConnectionStringTesting { get; set; }
        public static int MaxAnagramsForIp { get; set; }
        public static string AnagramicaApiUrl { get; set; }
    }
}
