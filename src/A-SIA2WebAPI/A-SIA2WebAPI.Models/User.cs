using A_SIA2WebAPI.Models.Attributes;
using System;

namespace A_SIA2WebAPI.Models
{
    public class User : Entity
    {
        [DatabasePropertyName(PropertyName = "email")]
        public string? Email { get; set; }

        [DatabasePropertyName(PropertyName = "hash")]
        public string? Hash { get; set; }

        [DatabasePropertyName(PropertyName = "first_name")]
        public string? FirstName { get; set; }

        [DatabasePropertyName(PropertyName = "last_name")]
        public string? LastName { get; set; }

        [DatabasePropertyName(PropertyName = "date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [DatabasePropertyName(PropertyName = "created_date")]
        public DateTime CreatedDate { get; set; }
    }
}