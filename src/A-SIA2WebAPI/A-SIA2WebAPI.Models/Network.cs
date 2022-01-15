using A_SIA2WebAPI.Models.Attributes;

namespace A_SIA2WebAPI.Models
{
    public class Network : Entity
    {
        [DatabasePropertyName(PropertyName = "name")]
        public string Name { get; set; } = "New Network";

        [DatabasePropertyName(PropertyName = "description")]
        public string Description { get; set; } = string.Empty;

        // Network specific settings

        [DatabasePropertyName(PropertyName = "anonymous")]
        public bool Anonymous { get; set; } = false;

        [DatabasePropertyName(PropertyName = "label_type")]
        public int LabelType { get; set; } = 0;

        // Defaults for Relations

        [DatabasePropertyName(PropertyName = "default_interval")]
        public int DefaultInterval { get; set; } = 1;

        [DatabasePropertyName(PropertyName = "default_offset")]
        public int DefaultOffset { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "default_influence")]
        public int DefaultInfluence { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "min_interval")]
        public int MinInterval { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "min_offset")]
        public int MinOffset { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "min_influence")]
        public int MinInfluence { get; set; } = 0;

        // Defaults for social nodes

        [DatabasePropertyName(PropertyName = "default_nodevalue")]
        public int DefaultNodevalue { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "default_reflection")]
        public int DefaultReflection { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "default_persistance")]
        public int DefaultPersistance { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "min_nodevalue")]
        public int MinNodevalue { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "max_nodevalue")]
        public int MaxNodevalue { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "min_reflection")]
        public int MinReflection { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "max_reflection")]
        public int MaxReflection { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "min_persistance")]
        public int MinPersistance { get; set; } = 0;

        [DatabasePropertyName(PropertyName = "max_persistance")]
        public int MaxPersistance { get; set; } = 0;
    }
}
