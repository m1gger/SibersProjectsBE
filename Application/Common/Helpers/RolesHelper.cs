using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public static class RolesHelper
    {
        public static UserRoleEnum GetUserRole(IList<string> roles) 
        {
            foreach (var r in roles) 
            {
                switch (r.ToLower()) 
                {
                    case "director":
                            return UserRoleEnum.Director;
                    case "projectmanager":
                        return UserRoleEnum.Manager;
                    case "employer":
                        return UserRoleEnum.Employer;

                }
            }
            throw new Exception("Unknown user role");
        } 
       
    }
}
