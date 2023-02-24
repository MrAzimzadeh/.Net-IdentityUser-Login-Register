using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.DTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Pasword and password repead is not match.")]
        public string PasswordConfig { get; set; }
        [Required(ErrorMessage ="Bura bos ola bilmez")]
        public string UserName { get; set; }

    }
}