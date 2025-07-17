using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class ProjectCompany
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; } 
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; } 
        public CompanyInProjectEnum InProjectEnum { get; set; }
    }
}
