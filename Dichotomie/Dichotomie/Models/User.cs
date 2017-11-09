using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dichotomie.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Mail { get; set; }
        [Required]
        [MaxLength(50)]
        public string Login { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(250)]
        public string Biography { get; set; }
        [Required]
        public float Reputation { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateJail { get; set; }

        // ForeignKey
        public List<Topic> Topics { get; set; }
        public List<Reply> Replies { get; set; }
    }
}
