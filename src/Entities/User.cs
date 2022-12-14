using SchoolOfDevs.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace SchoolOfDevs.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [NotMapped] //Annotation para não mapear essa variável como tabela no banco de dados
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public string CurrentPassword { get; set; }
        public TypeUser TypeUser { get; set; }
        public ICollection<Course> CoursesTeaching { get; set; }
        [JsonIgnore]
        public ICollection<Course> CoursesStudind { get; set; }
        [JsonIgnore]

        public List<StudentCourse> StudentCourses { get; set; }

    }
}
