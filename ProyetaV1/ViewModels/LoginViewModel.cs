using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProyetaV1.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [MinLength(15)]

        public string Username { get; set; }

        [Required]
        [MinLength(15)]

        public string Password { get; set; }
    }
}
