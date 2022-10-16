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
        public string Phone { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Your adress")]
        public string Adress { get; set; }
        [Required(ErrorMessage = "Please enter Your username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter Your password")]
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
