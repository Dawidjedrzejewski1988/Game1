using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Game.Models
{
    public class ForumThread
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        [StringLength(100, ErrorMessage = "Tytuł może mieć maksymalnie 100 znaków.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Treść jest wymagana.")]
        public string Content { get; set; }

        [Required]
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User? User { get; set; } 
        public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
    }
}
