using Microsoft.AspNetCore.Mvc.Rendering;
namespace StudentAPIConsumeWebsite.Models
{
    public class StudentPutModel
    {
        public string Name { get; set; }
        public Guid courseId { get; set; }
        public List<SelectListItem> Courses { get; set; }
    }
}
