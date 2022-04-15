using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Application.Models.DB2
{
    /// <summary>
    /// Result of loading data from DEL file into table
    /// </summary>
    public class LoadResult
    {
        /// <summary>
        /// Count of read rows
        /// </summary>
        /// <example>12</example>
        public long? ROWS_READ { get; set; }

        /// <summary>
        /// Count of skipped rows
        /// </summary>
        /// <example>2</example>
        public long? ROWS_SKIPPED { get; set; }

        /// <summary>
        /// Count of loaded rows
        /// </summary>
        /// <example>10</example>
        public long? ROWS_LOADED { get; set; }

        /// <summary>
        /// Count of rejected rows
        /// </summary>
        /// <example>0</example>
        public long? ROWS_REJECTED { get; set; }

        /// <summary>
        /// Count of deleted rows
        /// </summary>
        /// <example>0</example>
        public long? ROWS_DELETED { get; set; }

        /// <summary>
        /// Count of committed rows
        /// </summary>
        /// <example>10</example>
        public long? ROWS_COMMITTED { get; set; }

        /// <summary>
        /// Count of partitioned rows
        /// </summary>
        /// <example>0</example>
        public long? ROWS_PARTITIONED { get; set; }

        /// <summary>
        /// Number of agents
        /// </summary>
        /// <example>0</example>
        public long? NUM_AGENTINFO_ENTRIES { get; set; }

        /// <summary>
        /// SQL to retrieve load messages
        /// </summary>
        /// <example>SELECT SQLCODE, MSG FROM TABLE(SYSPROC.ADMIN_GET_MSGS('1618991872_2012791051_DB2INST1')) AS MSG</example>
        public string? MSG_RETRIEVAL { get; set; }

        /// <summary>
        /// SQL to delete load messages
        /// </summary>
        /// <example>CALL SYSPROC.ADMIN_REMOVE_MSGS('1618991872_2012791051_DB2INST1')</example>
        public string? MSG_REMOVAL { get; set; }
    }
}