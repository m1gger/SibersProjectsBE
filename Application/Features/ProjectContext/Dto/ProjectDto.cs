using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.ProjectContext.Dto
{
    class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int LeaderUserId { get; set; }
        public string LeaderUserName { get; set; } = string.Empty;
        public int CustomerCompamyId { get;set; }
        public string CustomerCompanyName { get; set; } = string.Empty;
        public int ContructorCompanyId { get; set; }
        public string ContructorCompanyName { get; set; } = string.Empty;
    }
}
