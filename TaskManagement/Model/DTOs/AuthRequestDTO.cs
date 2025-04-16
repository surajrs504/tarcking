﻿using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Model.DTOs
{
    public class    AuthRequestDTO
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
