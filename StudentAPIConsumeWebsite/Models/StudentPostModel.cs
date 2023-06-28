using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentAPIConsumeWebsite.Models
{
    public class StudentPostModel
    {
        public string Name { get; set; }
        public Guid CourseId { get; set; }
        public List<SelectListItem> Courses { get; set; }
    }
}
