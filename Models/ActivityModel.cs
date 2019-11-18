using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Exam2.Models
{
    public class ActivityModel
    {
        [Key]
        public int ActivityId {get;set;}
       
        public int UserId {get;set;}
        public User Coordinator {get;set;}
        [Required(ErrorMessage="Name of Activity is required")]        
        public string Title {get;set;}

        [Required (ErrorMessage="Date and Time are required")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Required (ErrorMessage="Duration is required")]
        public int Duration{get; set;}
        
        [Required (ErrorMessage="Duration is required")]
        public string DurationStr {get; set;}

        [Required (ErrorMessage="Description is required")]
        public string Description{get; set;}
        

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        
    
        public List<UserActivity> JoiningUser{ get; set;}
    }
}