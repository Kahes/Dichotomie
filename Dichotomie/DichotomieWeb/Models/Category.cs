using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Dichotomie.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        // ForeignKey
        [ForeignKey(nameof(ParentCategoryId))]
        public Category ParentCategory { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<Category> SubCategories { get; set; }
        public List<Topic> Topics { get; set; }
    }
}
