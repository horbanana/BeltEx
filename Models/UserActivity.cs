using System;
using System.Collections.Generic;

namespace Exam2.Models
{
    public class UserActivity
    {
        public int UserActivityId {get;set;}
        public int UserId {get; set;}
        public int ActivityId {get;set;}
        public User User {get; set;}
        public ActivityModel JoiningActivity {get;set;}
    }
    
}