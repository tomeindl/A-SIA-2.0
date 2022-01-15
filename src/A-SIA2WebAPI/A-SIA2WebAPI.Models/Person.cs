using A_SIA2WebAPI.Models.Attributes;
using System.Collections.Generic;

namespace A_SIA2WebAPI.Models
{
    public class Person : SocialNode
    {
        [DatabasePropertyName(PropertyName = "roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [DatabasePropertyName(PropertyName = "avatar_path")]
        public string AvatarPath { get; set; } = "";

    }
}
