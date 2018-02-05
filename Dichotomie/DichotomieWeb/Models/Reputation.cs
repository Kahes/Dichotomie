using Dichotomie.Models;
using DichotomieWeb.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DichotomieWeb.Models
{
    public class Reputation
    {
        [Key]
        public int ReputationId { get; set; }

        [Required]
        [MaxLength(1)]
        public int MarkValue { get; set; }

        // ForeignKey
        [ForeignKey(nameof(UserFK))]
        public ApplicationUser User { get; set; }
        public string UserFK { get; set; }

        // ForeignKey
        [ForeignKey(nameof(TopicFK))]
        public Topic Topic { get; set; }
        public int TopicFK { get; set; }
    }
}
