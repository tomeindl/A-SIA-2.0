using A_SIA2WebAPI.Models.Attributes;

namespace A_SIA2WebAPI.Models
{
    [DatabaseRelationType("INFLUENCES")]
    public class InfluencesRelation : Relation
    {
        [DatabasePropertyName(PropertyName = "influence")]
        public float Influence { get; set; }

        [DatabasePropertyName(PropertyName = "interval")]
        public int Interval { get; set; }

        [DatabasePropertyName(PropertyName = "offset")]
        public int Offset { get; set; }
    }
}
