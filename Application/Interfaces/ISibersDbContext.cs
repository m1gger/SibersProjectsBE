using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISibersDbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProjectUsers> ProjectUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<ProjectCompany> ProjectCompanies { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
        DbSet<TEntity> Set<TEntity>() where TEntity : class;


        Task<int> SaveChangesAsync(CancellationToken cancellationToken=default);
    }
}
