using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamGameHub.GameEngine.WebApi.Models
{
    /// <summary>
    /// This are information about a Node
    /// </summary>
    public class Node
    {
        /// <summary>
        /// The total price.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// The amount of available memory.
        /// </summary>
        public int AvailableMemory { get; set; }

        /// <summary>
        /// The amount available cores.
        /// </summary>        
        public int AvailableCores { get; set; }

        /// <summary>
        /// The amount of memory used.
        /// </summary>
        public int UsedMemory { get; set; }

        /// <summary>
        /// The amount of cores used.
        /// </summary>
        public int UsedCores { get; set; }

        /// <summary>
        /// The node instance type name.
        /// </summary>
        public string Type { get; set; }
    }
}
