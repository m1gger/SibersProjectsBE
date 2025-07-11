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
        public Project Project { get; set; } = new Project();
        public int CompanyId { get; set; }
        public Company Company { get; set; } = new Company();
        public CompanyInProjectEnum InProjectEnum { get; set; }
    }
}
