using Microsoft.AspNetCore.Mvc;
using Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Models;
using System.Diagnostics;

namespace Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Controllers
{
    public class StudentController : Controller
    {

        private readonly ISessionService _sessionService;
        public StudentController(ISessionService sessionService)
        {

            _sessionService = sessionService;
        }
        [HttpGet]
        public JsonResult IsEmailInUse(string Email)
        {
            var students = _sessionService.GetStudentList();  // Get the student list from the session
            bool isEmailTaken = students.Any(s => s.Email == Email);

            // Return true if the email is taken, otherwise false
            return Json(isEmailTaken ? "Email is already taken" : true);
        }
        public IActionResult Index()
        {

            var students = _sessionService.GetStudentList();
            return View(students);
        }

        // Create action to add a new student
        [HttpGet]
        public IActionResult Create()
        {
            var student = new Student(); // Initialize the model
            return View(student);
           
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                var students = _sessionService.GetStudentList();
                student.Id = students.Count == 0 ? 1 : students.Max(s => s.Id) + 1;
              
                students.Add(student);
                _sessionService.SetStudentList(students);
                return RedirectToAction("Index");
            }

            // If we reach here, it means validation failed
            return View(student);
        }

        // Edit action to update an existing student
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var students = _sessionService.GetStudentList();
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();
            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                var students = _sessionService.GetStudentList();
                var existingStudent = students.FirstOrDefault(s => s.Id == student.Id);
                if (existingStudent != null)
                {
                    existingStudent.Name = student.Name;
                    existingStudent.Email = student.Email;
                    _sessionService.SetStudentList(students);
                    return RedirectToAction("Index");
                }
            }

            // If we reach here, it means validation failed
            return View(student);
        }

        // Delete action to remove a student
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var students = _sessionService.GetStudentList();
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                students.Remove(student);
                _sessionService.SetStudentList(students);
            }
            return RedirectToAction("Index");
        }

        // Details action to show student details
        public IActionResult Details(int id)
        {
            var students = _sessionService.GetStudentList();
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();
            return View(student);
        }
    }
}
