using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum TaskStatusEnum
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        OnHold = 3,
        Testing =4,
        Cancelled = 5
    }
}
