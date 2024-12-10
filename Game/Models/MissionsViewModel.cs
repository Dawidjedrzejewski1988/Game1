using System.Collections.Generic;

namespace Game.Models
{
    public class MissionsViewModel
    {
        public Hero Hero { get; set; }
        public List<Quest> AvailableQuests { get; set; } = new List<Quest>();
        public List<Quest> AssignedQuests { get; set; } = new List<Quest>();
    }
}
