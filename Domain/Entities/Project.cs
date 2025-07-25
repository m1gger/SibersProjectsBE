﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int LeaderUserId { get; set; }
        public virtual User Leader { get; set; }
        public virtual List<ProjectUsers> ProjectUsers { get; set; } = new List<ProjectUsers>();
        public virtual List<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
        public virtual List<ProjectCompany> ProjectCompanies { get; set; } = new List<ProjectCompany>();
        public virtual List<ProjectDocument> ProjectDocuments { get; set; }= new List<ProjectDocument>();

    }
}
