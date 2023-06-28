using StudentsAPI.Models;

namespace StudentAPIConsumeWebsite.Models
{
    public class Details
    {
        public List<StudentDetails> GetStudents { get; set; }
        public List<Course> GetCourses { get; set; }
    }
}
