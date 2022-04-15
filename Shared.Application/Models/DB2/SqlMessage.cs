using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Application.Models.DB2
{
    /// <summary>
    /// SQL Message
    /// </summary>
    public class SqlMessage
    {
        /// <summary>
        /// SQL Code
        /// </summary>
        /// <example>SQL3109N</example>
        public string? SQLCODE { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        /// <example>The utility is beginning to load data from file "S3".</example>
        public string? MSG { get; set; }
    }
}