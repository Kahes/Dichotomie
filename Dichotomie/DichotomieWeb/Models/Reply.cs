﻿using DichotomieWeb.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dichotomie.Models
{
    public class Reply
    {
        [Key]
        public int ReplieId { get; set; }

        [Required]
        [MaxLength(250)]
        public string MainContent { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime CreationDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ModificationDate { get; set; }

        // ForeignKey
        [ForeignKey(nameof(UserFK))]
        public ApplicationUser User { get; set; }
        public string UserFK { get; set; }

        [ForeignKey(nameof(TopicFK))]
        public Topic Topic { get; set; }
        public int TopicFK { get; set; }
    }
}
