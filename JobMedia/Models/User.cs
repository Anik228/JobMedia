using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JobMedia.Models
{
    [Table("User")]
    public class User
    {
      
            public int Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

            public bool IsRecruiter { get; set; }
            public bool IsEmployer { get; set; }

            public bool IsAdmin{ get; set; }

            public bool RememberMe { get; set; }

            public bool IsDeleted { get; set; }

            public string Gender { get; set; }

    }
}