using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Exam2.Models
{
    public class User
    {
        public int UserId {get; set;}
        [Required(ErrorMessage="Name is required")]
        [MinLength(2, ErrorMessage="Minimum 2 characters!")]
        public string Name {get; set;}

        [Required (ErrorMessage="Email is requiered")]
        [EmailAddress]
        public string Email {get; set;}

        [Required (ErrorMessage="Password is Requiered")]
        [MinLength(8, ErrorMessage="Minimum 8 characters!")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must meet requirements. Requirements must have at least: 1 letter, 1 number, 1 special character.")]
//test1test1@!!!
        public string Password {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        [NotMapped]
        [Required (ErrorMessage="Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public List<UserActivity> JoiningActivity{ get; set;}
        public List<ActivityModel> CreatetedActivities { get; set; }
        
    }

}