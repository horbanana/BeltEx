using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam2.Models
{
    public class LUser
    {
        [Required(ErrorMessage="Email is not valid")]
        [EmailAddress]
        public string LEmail{get;set;}

        [Required (ErrorMessage="Password is not valid")]
        [DataType(DataType.Password)]
        public string LPassword{get;set;}
    }
}