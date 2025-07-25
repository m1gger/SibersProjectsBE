﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public virtual List<ProjectUsers> ProjectUsers { get; set; } = new List<ProjectUsers>();

        public virtual List<Project> ProjectsAsLeader { get; set; } = new List<Project>();
        public virtual List<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();

    }
}
