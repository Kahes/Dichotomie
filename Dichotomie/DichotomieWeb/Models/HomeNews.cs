using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DichotomieWeb.Models
{
    public class HomeNews
    {
        [Key]
        public int HomenewsId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string Content { get; set; }
    }
}
