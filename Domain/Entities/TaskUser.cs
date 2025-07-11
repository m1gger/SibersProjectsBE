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
        public ProjectTask Task { get; set; } = new ProjectTask();
        public int UserId { get; set; }
        public User User { get; set; } = new User();
        public bool IsLeader { get; set; } = false;
    }
}
