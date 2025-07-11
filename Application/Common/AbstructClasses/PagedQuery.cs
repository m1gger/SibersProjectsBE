using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.AbstructClasses
{
    public  abstract class PagedQuery
    {
        public int PageNumber { get; set; } = 1;

        /// <summary>
        ///    PageSize is the number of items to be displayed on a single page.
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
