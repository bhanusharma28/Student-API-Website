using StudentsAPI.Models;

namespace StudentAPIConsumeWebsite.Models
{
    public class StudentDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Course Courses { get; set; }
        public List<Course> GetCourses {  get; set; }
    }
}
