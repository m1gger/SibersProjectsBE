using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaskUser
    {
        public int TaskId { get; set; }
        public virtual ProjectTask Task { get; set; } 
        public int UserId { get; set; }
        public virtual User User { get; set; } 
        public bool IsLeader { get; set; } = false;
    }
}
