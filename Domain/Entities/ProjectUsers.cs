using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProjectUsers
    {
        public int UserId { get; set; }
        public User User;
        public int ProjectId { get; set; }
        public Project Project { get; set;   }
    }
}
