using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum UserRoleEnum
    {
        [Description("Employer")]
        Employer = 0,
        [Description("ProjectManager")]
        Manager = 1,
        [Description("Director")]
        Director = 2,
    }
}
