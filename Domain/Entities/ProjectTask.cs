using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;


namespace Domain.Entities
{
    public class ProjectTask
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; } 
        public virtual List<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();

        public TaskStatusEnum TaskStatus { get; set; } = TaskStatusEnum.NotStarted;


    }
}
