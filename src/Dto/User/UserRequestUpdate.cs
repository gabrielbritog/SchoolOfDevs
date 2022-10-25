using SchoolOfDevs.Enums;

namespace SchoolOfDevs.Dto.User
{
    public class UserRequestUpdate : UserRequest
    {
        public string CurrentPassword { get; set; }
    }
}
