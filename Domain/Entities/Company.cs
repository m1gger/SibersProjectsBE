using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual List<ProjectCompany> ProjectCompanies { get; set; } = new List<ProjectCompany>();


    }
}
