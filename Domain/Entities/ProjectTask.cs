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
        public Project Project { get; set; } = new Project();
        public int AssignedUserId { get; set; }
        public User AssignedUser { get; set; } = new User();
        public int LeaderUserId { get; set; }
        public User Leader { get; set; } = new User();
        public TaskStatusEnum TaskStatus { get; set; } = TaskStatusEnum.NotStarted;


    }
}
