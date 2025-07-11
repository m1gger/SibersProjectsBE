using Application.Common.AbstructClasses;
using Application.Common.Dto;
using Application.Features.TaskContext.Dto;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.TaskContext.Query
{
    public class GetAllTasksQuery : PagedQuery, IRequest<PagedDto<TaskDto>>
    {
        public int? ProjectId { get; set; }
        public string? Search { get; set; }
        public int? UserId { get; set; }
        public TaskStatusEnum? TaskStatusEnum { get; set; }
        public int? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
