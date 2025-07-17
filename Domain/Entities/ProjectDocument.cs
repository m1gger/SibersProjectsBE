using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProjectDocument
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string? Description { get; set; }
        public virtual Project Project { get; set; } 
        public string Path { get; set; }
    }
}
