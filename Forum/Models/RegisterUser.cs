using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forum.Models
{
    public class RegisterUser
    {
        [Key]
        public int id_user { get; set; }

        [Required]
        [StringLength(255)]
        public string user_name { get; set; }

        [Required]
        [StringLength(255)]
        public string pass { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("pass", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

        public bool role { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public bool? flag { get; set; }

        [StringLength(10)]
        public string role_detail { get; set; }
        [StringLength(255)]
        public string area { get; set; }

    }
}