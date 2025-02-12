using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace desafioAPI.DTO
{
    public class UserPostRequestBody
    {
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The password is required")]
        [Length(minimumLength:8, maximumLength:64, ErrorMessage = "The password must have between 8 and 64 characters")]
        public string Password { get; set; }
    }
}
