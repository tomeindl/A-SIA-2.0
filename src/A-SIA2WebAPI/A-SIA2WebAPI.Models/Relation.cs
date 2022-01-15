using A_SIA2WebAPI.Models.Attributes;
using System.Linq;

namespace A_SIA2WebAPI.Models
{
    public class Relation : Entity
    {
        public string? RelationType { get; set; }

        public long From { get; set; }

        public long To { get; set; }

        public Relation() : base()
        {
            RelationType = (GetType().GetCustomAttributes(true)
                .Where(a => a.GetType() == typeof(DatabaseRelationTypeAttribute))
                .FirstOrDefault() as DatabaseRelationTypeAttribute)?.Type;
        }
    }
}
