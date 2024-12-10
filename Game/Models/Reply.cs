using System;

namespace Game.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public int ThreadId { get; set; } 
        public string Content { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ForumThread Thread { get; set; }
        public virtual User User { get; set; }
    }
}
