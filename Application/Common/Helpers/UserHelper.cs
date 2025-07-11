using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Helpers
{
    public static class UserHelper
    {
        public static string GetUserFullName(this User user) 
        {
            var str = new StringBuilder();
            if (!string.IsNullOrEmpty(user.LastName))
            {
                str.Append(user.Name);
                str.Append(" ");

            }
            if (!string.IsNullOrEmpty(user.Name)) 
            {
                {
                    str.Append(user.Name);
                    str.Append(" ");

                }
            }
            if (!string.IsNullOrEmpty(user.Patronymic))
            {
                {
                    str.Append(user.Patronymic);
                }
            }

            return str.ToString();


        }
    }
}
