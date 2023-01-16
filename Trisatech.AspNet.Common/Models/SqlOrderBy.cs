using System;
using System.Collections.Generic;
using System.Text;

namespace Trisatech.AspNet.Common.Models
{
    public class SqlOrderBy
    {
        public SqlOrderBy()
        {
            Column = string.Empty;
            Type = string.Empty;
        }
        public string Column { get; set; }

        public string Type { get; set; }
    }
}
