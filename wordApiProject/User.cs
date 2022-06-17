﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace wordApiProject
{
    public class User
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Nickname { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string BussinessMail { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        
        
    }
}
