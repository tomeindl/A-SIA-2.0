using A_SIA2WebAPI.Models.Attributes;

namespace A_SIA2WebAPI.Models
{
    public class Group : SocialNode
    {
        [DatabasePropertyName(PropertyName = "collapsed")]
        public bool Collapsed { get; set; } = false;
    }
}
