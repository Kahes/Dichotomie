using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dichotomie.Models
{
    public class Category
    {
        [Key]
        public string CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } // Nom du jeu ou du service

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } // Game | Service
        
        public List<Topic> Topics { get; set; }
    }
}
