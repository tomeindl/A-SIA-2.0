namespace A_SIA2WebAPI.DAL.Neo4J
{
    internal class Neo4JSettings
    {
#if DEBUG

        public const string Uri = "bolt://80.109.99.55:7687";
        public const string Username = "neo4j";
        public const string Password = "serpent-miracle-pretty-cactus-modest-6996";

#else

        public const string Uri = "neo4j://miduskanich.com";
        public const string Username = "ASIA2";
        public const string Password = "V96LJjDWybgu8ySL";

#endif
    }
}
