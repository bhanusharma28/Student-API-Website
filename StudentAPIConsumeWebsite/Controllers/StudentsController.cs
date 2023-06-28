using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using StudentAPIConsumeWebsite.Api;
using StudentAPIConsumeWebsite.Models;
using StudentsAPI.Models;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace StudentAPIConsumeWebsite.Controllers
{
    public class StudentsController : Controller
    {
        private readonly IApiDataRequest apiDataRequest;

        public StudentsController(IApiDataRequest apiDataRequest)
        {
            this.apiDataRequest = apiDataRequest;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var resp = await apiDataRequest.ConsumeApi("https://localhost:44373/api/Students", null, "get", "json");

            Details Details = new Details();
            Details.GetStudents = JsonConvert.DeserializeObject<List<StudentDetails>>(resp.data);

            var response = await apiDataRequest.ConsumeApi("https://localhost:44373/api/Students/courses", null, "get", "json");            
            Details.GetCourses= JsonConvert.DeserializeObject<List<Course>>(response.data);

            return View(Details);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var courses = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/courses", null, "get", "json");
            var courseslist = JsonConvert.DeserializeObject<List<Course>>(courses.data);
            StudentPostModel postModel = new StudentPostModel();
            // Populate dropdown options
            postModel.Courses = courseslist.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CourseDescription,
            }).ToList();
            return View(postModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(StudentPostModel studentDetails)
        {
            var json = string.Empty;
            json = JsonConvert.SerializeObject(studentDetails);
            var response = await apiDataRequest.ConsumeApi("https://localhost:44373/api/Students", json, "post", "json");
            if(response.HttpStatusCode == HttpStatusCode.OK)
            {
                TempData["SuccessMessage"] = "Student added successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong or Student already exists with same details.";
            }
            return RedirectToAction("Add");
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            return View();
        }

        //Converted this method for SOAP Call
        //[HttpPost]
        //public async Task<IActionResult> AddCourse(CoursePostModel coursePost)
        //{
        //    var json = string.Empty;
        //    json = JsonConvert.SerializeObject(coursePost);
        //    var response = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/addCourse", json, "post", "");
        //    if (response.HttpStatusCode == HttpStatusCode.OK)
        //    {
        //        TempData["SuccessMessage"] = "Course added successfully!";
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Course already exisits with the same entries or Something went wrong.";
        //    }
        //    return RedirectToAction("AddCourse");
        //}

        [HttpPost]
        public async Task<IActionResult> AddCourse(CoursePostModel coursePost)
        {
            // Convert courseRequest to XML
            string xmlInput;

            var serializer = new XmlSerializer(typeof(CoursePostModel));
            using (var memoryStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings { Encoding = Encoding.UTF8 }))
                {
                    serializer.Serialize(xmlWriter, coursePost);
                }
                xmlInput = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            var response = await apiDataRequest.ConsumeApi("https://localhost:44373/api/SoapStudents", xmlInput, "post", "xml");
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                TempData["SuccessMessage"] = "Course added successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Something went wrong or Course already exists with the same entries.";
            }
            return RedirectToAction("AddCourse");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var resp = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/{id}", null, "get", "json");
            StudentDetails student = JsonConvert.DeserializeObject<StudentDetails>(resp.data);
            StudentPutModel studentPut = new StudentPutModel();
            studentPut.Name = student.Name;
            studentPut.courseId = student.Courses.Id;

            var courses = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/courses", null, "get", "json");
            var courseslist = JsonConvert.DeserializeObject<List<Course>>(courses.data);

            // Populate dropdown options
            studentPut.Courses = courseslist.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.CourseDescription,
            }).ToList();

            return View(studentPut);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentPostModel studentPostModel, int id)
        {
            var json = string.Empty;
            json = JsonConvert.SerializeObject(studentPostModel);
            var res = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/{id}", json, "put", "json");
            if(res.HttpStatusCode == HttpStatusCode.OK)
            {
                TempData["UpdateSuccess"] = "Student Updated successfully!";
            }
            else
            {
                TempData["UpdateError"] = "Student Update failed. Something went wrong or Data already exists.";
            }
            return RedirectToAction("Edit");
        }

        [HttpGet]
        public async Task<IActionResult> EditCourse(Guid id)
        {
            var response = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/courses/{id}", null, "get", "json");
            Course course = JsonConvert.DeserializeObject<Course>(response.data);
            CoursePutModel coursePut = new CoursePutModel
            {
                CourseName = course.CourseName,
                CourseDescription = course.CourseDescription,
                Specialization = course.Specialization
            };
            return View(coursePut);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(CoursePostModel coursePost, Guid id)
        {
            var json = string.Empty;
            json = JsonConvert.SerializeObject(coursePost);
            var response = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/editCourse/{id}", json, "put", "json");
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                TempData["UpdateSuccess"] = "Course Updated Successfully!";
            }
            else
            {
                TempData["UpdateError"] = "Something went wrong or Course Already exists with same details."; 
            }
            return RedirectToAction("EditCourse");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/{id}", null, "delete", "json");
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                TempData["DeleteSuccess"] = "Deleted successfully!";
            }
            else
            {
                TempData["DeleteError"] = "Deletion failed. Something went wrong.";
            }
            return RedirectToAction("ManageStudents");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var response = await apiDataRequest.ConsumeApi($"https://localhost:44373/api/Students/courses/{id}", null, "delete", "json");
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                TempData["DeleteSuccess"] = "Deleted successfully!";
            }
            else
            {
                TempData["DeleteError"] = "Deletion failed. Something went wrong.";
            }
            return RedirectToAction("ManageCourses");
        }

        [HttpGet]
        public async Task<IActionResult> ManageStudents()
        {
            var resp = await apiDataRequest.ConsumeApi("https://localhost:44373/api/Students", null, "get", "json");
            Details details = new Details();
            details.GetStudents = JsonConvert.DeserializeObject<List<StudentDetails>>(resp.data);
            return View(details);
        }

        [HttpPost]
        public async Task<IActionResult> ManageStudents(string? str)
        {
            var resp = await apiDataRequest.ConsumeApi("https://localhost:44373/api/Students", null, "get", "json");
            Details details = new Details();
            details.GetStudents = JsonConvert.DeserializeObject<List<StudentDetails>>(resp.data);

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                details.GetStudents = details.GetStudents.Where(s => s.Name.StartsWith(str, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(details);
        }

        [HttpGet]
        public async Task<IActionResult> ManageCourses()
        {
            var response = await apiDataRequest.ConsumeApi("https://localhost:44373/api/Students/courses", null, "get", "json");
            Details details = new Details();
            details.GetCourses = JsonConvert.DeserializeObject<List<Course>>(response.data);
            return View(details);
        }

        [HttpPost]
        public async Task<IActionResult> ManageCourses(string? str)
        {
            var response = await apiDataRequest.ConsumeApi("https://localhost:44373/api/Students/courses", null, "get", "json");
            Details details = new Details();
            details.GetCourses = JsonConvert.DeserializeObject<List<Course>>(response.data);

            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                details.GetCourses = details.GetCourses.Where(s => s.CourseName.StartsWith(str, StringComparison.OrdinalIgnoreCase) ||
                                                              s.CourseDescription.StartsWith(str, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(details);
        }
    }
}