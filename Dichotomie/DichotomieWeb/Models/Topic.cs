using DichotomieWeb.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dichotomie.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }

        [Required]
        [MaxLength(250)]
        public string MainContent { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(50)]
        public string TradeSystem { get; set; }
        [Required]
        public int State { get; set; }
        [Required]
        public int Pin { get; set; }
        [Required]
        public int CurrencyUsed  { get; set; }
        [Required]
        public float Rating { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ModificationDate { get; set; }

        // ForeignKey
        [ForeignKey(nameof(UserFK))]
        public ApplicationUser User { get; set; }
        public string UserFK { get; set; }

        [ForeignKey(nameof(CategoryFK))]
        public Category Category { get; set; }
        public int CategoryFK { get; set; }

        public List<Reply> Replies { get; set; }
    }
}
