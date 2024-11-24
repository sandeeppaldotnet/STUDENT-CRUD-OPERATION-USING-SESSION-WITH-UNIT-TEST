using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Student_Crud_Operation_Using_Session_State_In_Asp.net_Core_MVC.Models
{
    public class Student
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [Remote("IsEmailInUse", "Student", ErrorMessage = "Email is already taken")]
        public string Email { get; set; }
    }
}
