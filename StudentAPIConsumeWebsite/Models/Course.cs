namespace StudentsAPI.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string Specialization { get; set; }
    }
}
