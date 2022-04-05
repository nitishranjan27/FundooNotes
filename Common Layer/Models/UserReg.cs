using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common_Layer.Models
{
    public class UserReg
    {
        [Required]
        [RegularExpression(@"^[A-Z]{1}[a-z]{2,}$")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{1}[a-z]{2,}$")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[a-z]+([._+-][0-9A-Za-z]+)*[@][0-9A-Za-z]+.[a-zA-Z]{2,3}(.[a-zA-Z]{2,3})?$")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{1}[A-Z a-z]{3,}[!*@#$%^&+=]?[0-9]{1,}$")]
        public string Password { get; set; }
    }
}
