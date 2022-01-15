using System.Collections.Generic;

namespace A_SIA2WebAPI.Models
{
    public class NetworkStructure
    {
        public List<Person> People { get; set; } = new();
        public List<InfluenceMatrixEntry> InfluenceMatrix { get; set; } = new();
        public List<GroupsEntry> Groups { get; set; } = new();

        public class InfluenceMatrixEntry
        {
            public Person? From { get; set; }
            public Person? To { get; set; }
            public InfluencesRelation? Relation { get; set; }
        }

        public class GroupsEntry
        {
            public Group? Group { get; set; }
            public List<SocialNode> Nodes { get; set; } = new();
        }
    }
}