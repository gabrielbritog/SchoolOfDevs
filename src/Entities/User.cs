using SchoolOfDevs.Enums;
using System.Reflection.Metadata.Ecma335;

namespace SchoolOfDevs.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public TyperUser TyperUser { get; set; }

    }
}
