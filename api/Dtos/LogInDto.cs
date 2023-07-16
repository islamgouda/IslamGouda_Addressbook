﻿using System.ComponentModel.DataAnnotations;

namespace api.Dtos
{
    public class LogInDto
    {
        
            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string? Password { get; set; }
       
    }
}
