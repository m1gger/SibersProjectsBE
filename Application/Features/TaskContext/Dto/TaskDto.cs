using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TaskContext.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string TaskStatus { get; set; } = string.Empty;
        public int LeaderUserId { get; set; }
        public string LeaderName { get; set; } = string.Empty;
        public int EmployeeUserId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;



    }
}
