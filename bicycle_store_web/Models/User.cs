using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace bicycle_store_web
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Your full name")]
        public string FullName { get; set; }
        [Phone]
        [Required(ErrorMessage = "Please enter Your phone number")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter Your email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Your adress")]
        public string Adress { get; set; }
        public byte[] Photo { get; set; }
        [Required(ErrorMessage = "Please enter Your username")]
        public string Username { get; set; }
        [MinLength(4)]
        [Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
