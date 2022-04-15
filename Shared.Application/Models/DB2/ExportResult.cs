using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Application.Models.DB2
{
    /// <summary>
    /// Result of exporting SQL to DEL file
    /// </summary>
    public class ExportResult
    {
        /// <summary>
        /// Count of exported rows
        /// </summary>
        public long? ROWS_EXPORTED { get; set; }

        /// <summary>
        /// SQL to get export messages
        /// </summary>
        public string? MSG_RETRIEVAL { get; set; }

        /// <summary>
        /// SQL to delete export messages
        /// </summary>
        public string? MSG_REMOVAL { get; set; }
    }
}